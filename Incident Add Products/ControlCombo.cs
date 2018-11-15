using RightNow.AddIns.AddInViews;
using RightNow.AddIns.Common;
using System;
using System.Windows.Forms;
using RestSharp;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.ServiceModel;
using Incident_Add_Products.SOAPICCS;
using System.ServiceModel.Channels;
using Newtonsoft.Json;
using System.Linq;

namespace Incident_Add_Products
{
    public partial class ControlCombo : UserControl
    {
        public bool inDesignMode;
        public List<RootObject> InsertedServices { get; set; }
        public IRecordContext recordContext;
        public IGlobalContext globalContext;
        public IIncident Incident { get; set; }
        public IGenericObject SerObject;
        public IGenericObject ItiObject;
        public int AirportID { get; set; }
        public string AirtportText { get; set; }
        public string SRType { get; set; }
        public int IdIncident { get; set; }
        public int IdItinerary { get; set; }
        public RightNowSyncPortClient client;
        public string result { get; set; }
        public string ItemClaveSat { get; set; }
        public string ParticipacionCobro { get; set; }
        public string CuentaGasto { get; set; }
        public string CobroParticipacionNj { get; set; }
        public string Pagos { get; set; }
        public string ClasificacionPago { get; set; }
        public string Informativo { get; set; }
        public string Component { get; set; }
        public string Pax { get; set; }
        public string Categories { get; set; }
        public string ItemNumberParent { get; set; }
        public string ItemDesc { get; set; }
        public List<Items> items { get; set; }



        public ControlCombo()
        {
            InitializeComponent();
        }
        public ControlCombo(bool inDesignMode, IRecordContext recordContext, IGlobalContext globalContext) : this()
        {
            this.inDesignMode = inDesignMode;
            this.recordContext = recordContext;
            this.globalContext = globalContext;
        }
        internal void LoadData()
        {
            if (Init())
            {
                try
                {
                    InsertedServices = new List<RootObject>();
                    IdItinerary = 0;
                    GetAirport();
                    SRType = GetSRType();
                    AirtportText = GetAirportText();
                    if (!String.IsNullOrEmpty(SRType))
                    {
                        GetPData(AirtportText, SRType, "");
                    }
                }
                catch (Exception ex)
                {
                    globalContext.LogMessage(ex.Message);
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ItiObject != null || Incident != null)
            {
                ItemNumberParent = CboProductos.SelectedValue.ToString();
                ItemDesc = CboProductos.Text;
                GetPData(AirtportText, SRType, CboProductos.SelectedValue.ToString());
            }
        }

        public void GetPData(string AirportText, string Srtype, string ItemN)
        {
            try
            {
                string envelope = "<soapenv:Envelope" +
                 "   xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"" +
                 "   xmlns:typ=\"http://xmlns.oracle.com/apps/scm/productModel/items/itemServiceV2/types/\"" +
                 "   xmlns:typ1=\"http://xmlns.oracle.com/adf/svc/types/\">" +
                 "<soapenv:Header/>" +
                 "<soapenv:Body>" +
                 "<typ:findItem>" +
                 "<typ:findCriteria>" +
                 "<typ1:fetchStart>0</typ1:fetchStart>" +
                 "<typ1:fetchSize>-1</typ1:fetchSize>" +
                 "<typ1:filter>" +
                 "<typ1:group>";
                if (!string.IsNullOrEmpty(ItemN))
                {
                    envelope += "<typ1:item>" +
                     "<typ1:conjunction>And</typ1:conjunction>" +
                     "<typ1:upperCaseCompare>true</typ1:upperCaseCompare>" +
                     "<typ1:attribute>ItemNumber</typ1:attribute>" +
                     "<typ1:operator>CONTAINS</typ1:operator>" +
                     "<typ1:value>" + ItemN + "</typ1:value>" +
                     "</typ1:item>";
                }
                envelope += "<typ1:item>" +
                 "<typ1:conjunction>And</typ1:conjunction>" +
                 "<typ1:upperCaseCompare>true</typ1:upperCaseCompare>" +
                 "<typ1:attribute>OrganizationCode</typ1:attribute>" +
                 "<typ1:operator>=</typ1:operator>";

                if (SRType == "SENEAM" || SRType == "PERMISOS")
                {
                    envelope += "<typ1:value>MTS_ITEM</typ1:value>";
                }
                else
                {
                    envelope += "<typ1:value>IO_AEREO_" + AirportText + "</typ1:value>";
                }
                envelope += "</typ1:item>" +
                 "<typ1:item>" +
                 "<typ1:conjunction>And</typ1:conjunction>" +
                 "<typ1:upperCaseCompare>true</typ1:upperCaseCompare>" +
                 "<typ1:attribute>ItemCategory</typ1:attribute>" +
                 "<typ1:nested>" +
                 "<typ1:group>" +
                 "<typ1:item>" +
                 "<typ1:conjunction>And</typ1:conjunction>" +
                 "<typ1:upperCaseCompare>true</typ1:upperCaseCompare>" +
                 "<typ1:attribute>CategoryName</typ1:attribute>" +
                 "<typ1:operator>CONTAINS</typ1:operator>" +
                 "<typ1:value>" + Srtype + "</typ1:value>" +
                 "</typ1:item>" +
                 "</typ1:group>" +
                 "</typ1:nested>" +
                 "</typ1:item>";
                if (SRType == "CATERING")
                {
                    envelope +=
                         "<typ1:item>" +
                 "<typ1:conjunction>And</typ1:conjunction>" +
                 "<typ1:attribute>ItemDFF</typ1:attribute>" +
                 "<typ1:nested>" +
                 "<typ1:group>" +
                 "<typ1:item>" +
                 "<typ1:conjunction>And</typ1:conjunction>" +
                 "<typ1:upperCaseCompare>true</typ1:upperCaseCompare>" +
                 "<typ1:attribute>xxInformativo</typ1:attribute>" +
                 "<typ1:operator>=</typ1:operator>" +
                 "<typ1:value>NO</typ1:value>" +
                 "</typ1:item>" +
                 "</typ1:group>" +
                 "</typ1:nested>" +
                 "</typ1:item>";
                }
                envelope += "</typ1:group>" +
                                 "</typ1:filter>" +
                                 "<typ1:findAttribute>OrganizationCode</typ1:findAttribute>" +
                                 "<typ1:findAttribute>ItemNumber</typ1:findAttribute>" +
                                 "<typ1:findAttribute>ItemDescription</typ1:findAttribute>";
                if (!string.IsNullOrEmpty(ItemN))
                {
                    envelope += "<typ1:findAttribute>ItemDFF</typ1:findAttribute>";
                }
                envelope += "</typ:findCriteria>" +
                 "<typ:findControl>" +
                 "<typ1:retrieveAllTranslations>true</typ1:retrieveAllTranslations>" +
                 "</typ:findControl>" +
                 "</typ:findItem>" +
                 "</soapenv:Body>" +
                 "</soapenv:Envelope>";
                globalContext.LogMessage(envelope);
                byte[] byteArray = Encoding.UTF8.GetBytes(envelope);
                // Construct the base 64 encoded string used as credentials for the service call
                byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes("itotal" + ":" + "Oracle123");
                string credentials = System.Convert.ToBase64String(toEncodeAsBytes);
                // Create HttpWebRequest connection to the service
                HttpWebRequest request =
                 (HttpWebRequest)WebRequest.Create("https://egqy-test.fa.us6.oraclecloud.com:443/fscmService/ItemServiceV2");
                // Configure the request content type to be xml, HTTP method to be POST, and set the content length
                request.Method = "POST";
                request.ContentType = "text/xml;charset=UTF-8";
                request.ContentLength = byteArray.Length;
                // Configure the request to use basic authentication, with base64 encoded user name and password, to invoke the service.
                request.Headers.Add("Authorization", "Basic " + credentials);
                // Set the SOAP action to be invoked; while the call works without this, the value is expected to be set based as per standards
                request.Headers.Add("SOAPAction", "http://xmlns.oracle.com/apps/scm/productModel/items/itemServiceV2/findItem");
                // Write the xml payload to the request
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                // Write the xml payload to the request
                XDocument doc;
                XmlDocument docu = new XmlDocument();
                // Get the response and process it; In this example, we simply print out the response XDocument doc;
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        doc = XDocument.Load(stream);
                        result = doc.ToString();
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(result);
                        XmlNamespaceManager nms = new XmlNamespaceManager(xmlDoc.NameTable);
                        nms.AddNamespace("env", "http://schemas.xmlsoap.org/soap/envelope/");
                        nms.AddNamespace("wsa", "http://www.w3.org/2005/08/addressing");
                        nms.AddNamespace("typ", "http://xmlns.oracle.com/apps/scm/productModel/items/itemServiceV2/types/");
                        nms.AddNamespace("ns0", "http://xmlns.oracle.com/adf/svc/types/");
                        nms.AddNamespace("ns1", "http://xmlns.oracle.com/apps/scm/productModel/items/itemServiceV2/");
                        if (!String.IsNullOrEmpty(ItemN))
                        {
                            XmlNode desiredNode = xmlDoc.SelectSingleNode("//ns1:ItemDFF", nms);
                            if (desiredNode.HasChildNodes)
                            {
                                for (int i = 0; i < desiredNode.ChildNodes.Count; i++)
                                {
                                    if (desiredNode.ChildNodes[i].LocalName == "xxParticipacionCobro")
                                    {
                                        ParticipacionCobro = desiredNode.ChildNodes[i].InnerText == "SI" ? "1" : "0";
                                    }
                                    if (desiredNode.ChildNodes[i].LocalName == "xxCobroParticipacionNj")
                                    {
                                        CobroParticipacionNj = desiredNode.ChildNodes[i].InnerText == "SI" ? "1" : "0";
                                    }
                                    if (desiredNode.ChildNodes[i].LocalName == "xxPagos")
                                    {
                                        Pagos = desiredNode.ChildNodes[i].InnerText;
                                    }
                                    if (desiredNode.ChildNodes[i].LocalName == "xxClasificacionPago")
                                    {
                                        ClasificacionPago = desiredNode.ChildNodes[i].InnerText;
                                    }
                                    if (desiredNode.ChildNodes[i].LocalName == "cuentaGastoCx")
                                    {
                                        CuentaGasto = desiredNode.ChildNodes[i].InnerText;
                                    }
                                    if (desiredNode.ChildNodes[i].LocalName == "xxInformativo")
                                    {
                                        Informativo = desiredNode.ChildNodes[i].InnerText == "SI" ? "1" : "0";
                                    }
                                    if (desiredNode.ChildNodes[i].LocalName == "xxPaqueteInv")
                                    {
                                        Pax = desiredNode.ChildNodes[i].InnerText == "SI" ? "1" : "0";
                                    }
                                }
                            }
                            Categories = GetCategories(ItemN, AirportText);
                            LlenarValoresServicio();
                        }
                        else
                        {
                            items = new List<Items>();
                            Dictionary<string, string> test = new Dictionary<string, string>();
                            AutoCompleteStringCollection dataCollection = new AutoCompleteStringCollection();
                            XmlNodeList nodeList = xmlDoc.SelectNodes("//ns0:Value", nms);
                            foreach (XmlNode node in nodeList)
                            {
                                Items item = new Items();
                                if (node.HasChildNodes)
                                {
                                    if (node.LocalName == "Value")
                                    {
                                        XmlNodeList nodeListvalue = node.ChildNodes;
                                        foreach (XmlNode nodeValue in nodeListvalue)
                                        {
                                            if (nodeValue.LocalName == "ItemNumber")
                                            {
                                                item.ItemNumber = nodeValue.InnerText;
                                            }
                                            if (nodeValue.LocalName == "ItemDescription")
                                            {
                                                item.Description = nodeValue.InnerText;

                                            }
                                        }
                                    }
                                    if (item.ItemNumber != "AGASIAS0270" && item.ItemNumber != "JFUEIAS0269" && item.ItemNumber != "AGASIAS0011" && item.ItemNumber != "JFUEIAS0010" && item.ItemNumber != "IAFMUAS0271" && item.ItemNumber != "AFMURAS0016")
                                    {
                                        test.Add(item.ItemNumber, item.Description);
                                        items.Add(item);
                                    }
                                }
                            }
                            if (test.Count > 0)
                            {
                                if (SRType == "FBO")
                                {
                                    if (GetCountryItinerary(IdItinerary))
                                    {
                                        test.Remove("IISNNAP248");
                                    }
                                    else
                                    {
                                        test.Remove("DISONAP249");
                                    }
                                }
                                items.OrderBy(o => o.Description);
                                dataGridServicios.DataSource = items;
                                dataGridServicios.Columns[0].Visible = false;
                                dataGridServicios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                                CboProductos.DataSource = new BindingSource(test.OrderBy(item => item.Value), null);
                                CboProductos.DisplayMember = "Value";
                                CboProductos.ValueMember = "Key";
                                string value = ((KeyValuePair<string, string>)CboProductos.SelectedItem).Value;
                                CboProductos.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                CboProductos.AutoCompleteSource = AutoCompleteSource.CustomSource;
                                AutoCompleteStringCollection combData = dataCollection;
                                CboProductos.AutoCompleteCustomSource = combData;
                            }
                            else
                            {
                                CboProductos.Enabled = false;
                                btnAdd.Enabled = false;
                            }
                        }
                    }
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);

            }
        }
        public bool Init()
        {
            try
            {
                bool result = false;
                EndpointAddress endPointAddr = new EndpointAddress(globalContext.GetInterfaceServiceUrl(ConnectServiceType.Soap));
                // Minimum required
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportWithMessageCredential);
                binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
                binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
                binding.MaxReceivedMessageSize = 1048576; //1MB
                binding.SendTimeout = new TimeSpan(0, 10, 0);
                // Create client proxy class
                client = new RightNowSyncPortClient(binding, endPointAddr);
                // Ask the client to not send the timestamp
                BindingElementCollection elements = client.Endpoint.Binding.CreateBindingElements();
                elements.Find<SecurityBindingElement>().IncludeTimestamp = false;
                client.Endpoint.Binding = new CustomBinding(elements);
                // Ask the Add-In framework the handle the session logic
                globalContext.PrepareConnectSession(client.ChannelFactory);
                if (client != null)
                {
                    result = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;

            }
        }
        public void GetAirport()
        {
            ItiObject = recordContext.GetWorkspaceRecord("CO$Itinerary") as IGenericObject;
            Incident = recordContext.GetWorkspaceRecord(WorkspaceRecordType.Incident) as IIncident;
            if (ItiObject != null)
            {

                IdItinerary = ItiObject.Id;
                IList<IGenericField> fields = ItiObject.GenericFields;
                foreach (IGenericField genField in fields)
                {
                    if (genField.Name == "ArrivalAirport")
                    {
                        AirportID = Convert.ToInt32(genField.DataValue.Value);
                    }
                }
            }
            else if (Incident != null)
            {
                IList<ICustomAttribute> cfVals = Incident.CustomAttributes;
                foreach (ICustomAttribute custom in cfVals)
                {
                    if (custom.GenericField.Name == "CO$Airports")
                    {
                        AirportID = Convert.ToInt32(custom.GenericField.DataValue.Value);
                    }
                }
            }
        }
        public string GetSRType()
        {
            string SRTYPE = "";
            if (ItiObject != null)
            {
                IList<IGenericField> fields = ItiObject.GenericFields;
                foreach (IGenericField genField in fields)
                {
                    if (genField.Name == "Incident1")
                    {
                        IdIncident = Convert.ToInt32(genField.DataValue.Value);
                    }
                }
            }
            else if (Incident != null)
            {
                IdIncident = Incident.ID;
            }
            if (IdIncident != 0)
            {
                ClientInfoHeader clientInfoHeader = new ClientInfoHeader();
                APIAccessRequestHeader aPIAccessRequest = new APIAccessRequestHeader();
                clientInfoHeader.AppID = "Query Example";
                String queryString = "SELECT I.Customfields.c.sr_type.LookupName FROM Incident I WHERE id=" + IdIncident + "";
                client.QueryCSV(clientInfoHeader, aPIAccessRequest, queryString, 10000, "|", false, false, out CSVTableSet queryCSV, out byte[] FileData);
                foreach (CSVTable table in queryCSV.CSVTables)
                {
                    String[] rowData = table.Rows;
                    foreach (String data in rowData)
                    {
                        SRTYPE = data;
                    }
                }
            }
            switch (SRTYPE)
            {
                case "Catering":
                    SRTYPE = "CATERING";
                    break;
                case "FCC":
                    SRTYPE = "FCC";
                    break;
                case "FBO":
                    SRTYPE = "FBO";
                    break;
                case "Fuel":
                    SRTYPE = "FUEL";
                    break;
                case "Hangar Space":
                    SRTYPE = "GYCUSTODIA";
                    break;
                case "SENEAM Fee":
                    SRTYPE = "SENEAM";
                    break;
                case "Permits":
                    SRTYPE = "PERMISOS";
                    break;
            }
            return SRTYPE;
        }
        public bool GetCountryItinerary(int Itinerary)
        {
            bool res = true;
            ClientInfoHeader clientInfoHeader = new ClientInfoHeader();
            APIAccessRequestHeader aPIAccessRequest = new APIAccessRequestHeader();
            clientInfoHeader.AppID = "Query Example";
            String queryString = "SELECT FromAirport.Country.IsoCode,ToAirport.Country.IsoCode FROM CO.Itinerary WHERE Id = " + Itinerary;
            client.QueryCSV(clientInfoHeader, aPIAccessRequest, queryString, 1, "|", false, false, out CSVTableSet queryCSV, out byte[] FileData);
            foreach (CSVTable table in queryCSV.CSVTables)
            {

                String[] rowData = table.Rows;
                foreach (String data in rowData)
                {
                    Char delimiter = '|';
                    String[] substring = data.Split(delimiter);
                    if (substring[0] != "MX" || substring[1] != "MX")
                    {
                        res = false;
                    }
                }
            }
            return res;
        }
        public string GetAirportText()
        {
            string air = "";
            ClientInfoHeader clientInfoHeader = new ClientInfoHeader();
            APIAccessRequestHeader aPIAccessRequest = new APIAccessRequestHeader();
            clientInfoHeader.AppID = "Query Example";
            String queryString = "SELECT ICAOCode,IATACode FROM CO.Airports WHERE ID =" + AirportID;
            client.QueryCSV(clientInfoHeader, aPIAccessRequest, queryString, 10000, ",", false, false, out CSVTableSet queryCSV, out byte[] FileData);
            foreach (CSVTable table in queryCSV.CSVTables)
            {
                String[] rowData = table.Rows;
                foreach (String data in rowData)
                {
                    Char delimiter = ',';
                    String[] substrings = data.Split(delimiter);
                    air = substrings[0] + "_" + substrings[1];
                    air = air.Trim();
                }
            }
            return air;
        }
        public void LlenarValoresServicio()
        {
            SerObject = recordContext.GetWorkspaceRecord("CO$Services") as IGenericObject;
            IList<IGenericField> fields = SerObject.GenericFields;
            foreach (IGenericField genField in fields)
            {
                if (genField.Name == "CuentaGasto")
                {
                    genField.DataValue.Value = CuentaGasto;
                }
                if (genField.Name == "CobroParticipacionNj")
                {
                    genField.DataValue.Value = CobroParticipacionNj;
                }
                if (genField.Name == "ItemDescription")
                {
                    genField.DataValue.Value = ItemDesc.Trim();
                }
                if (genField.Name == "ItemNumber")
                {
                    genField.DataValue.Value = ItemNumberParent;
                }
                if (genField.Name == "Pagos")
                {
                    genField.DataValue.Value = Pagos;
                }
                if (genField.Name == "ParticipacionCobro")
                {
                    genField.DataValue.Value = ParticipacionCobro;
                }
                if (genField.Name == "ClasificacionPagos")
                {
                    genField.DataValue.Value = ClasificacionPago;
                }
                if (genField.Name == "Airport")
                {
                    genField.DataValue.Value = AirtportText;
                }
                if (genField.Name == "Paquete")
                {
                    genField.DataValue.Value = Pax;
                }
                if (genField.Name == "Informativo")
                {
                    genField.DataValue.Value = Informativo;
                }
                if (genField.Name == "Categories")
                {
                    genField.DataValue.Value = Categories;
                }
            }

        }
        public string GetCategories(string ItemN, string Airport)
        {
            try
            {
                string cats = "";
                string envelope = "<soapenv:Envelope" +
                "   xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"" +
                "   xmlns:typ=\"http://xmlns.oracle.com/apps/scm/productModel/items/itemServiceV2/types/\"" +
                "   xmlns:typ1=\"http://xmlns.oracle.com/adf/svc/types/\">" +
                "<soapenv:Header/>" +
                "<soapenv:Body>" +
       "<typ:findItem>" +
           "<typ:findCriteria>" +
               "<typ1:fetchStart>0</typ1:fetchStart>" +
               "<typ1:fetchSize>-1</typ1:fetchSize>" +
               "<typ1:filter>" +
                   "<typ1:group>" +
                       "<typ1:item>" +
                           "<typ1:conjunction>And</typ1:conjunction>" +
                           "<typ1:upperCaseCompare>true</typ1:upperCaseCompare>" +
                           "<typ1:attribute>ItemNumber</typ1:attribute>" +
                           "<typ1:operator>=</typ1:operator>" +
                           "<typ1:value>" + ItemN + "</typ1:value>" +
                       "</typ1:item>" +
                       "<typ1:item>" +
                           "<typ1:conjunction>And</typ1:conjunction>" +
                           "<typ1:upperCaseCompare>true</typ1:upperCaseCompare>" +
                           "<typ1:attribute>OrganizationCode</typ1:attribute>" +
                           "<typ1:operator>=</typ1:operator>" +
                           "<typ1:value>IO_AEREO_" + Airport + "</typ1:value>" +
                       "</typ1:item>" +
                   "</typ1:group>" +
               "</typ1:filter>" +
               "<typ1:findAttribute>ItemCategory</typ1:findAttribute>" +
           "</typ:findCriteria>" +
           "<typ:findControl>" +
               "<typ1:retrieveAllTranslations>true</typ1:retrieveAllTranslations>" +
           "</typ:findControl>" +
       "</typ:findItem>" +
   "</soapenv:Body>" +
"</soapenv:Envelope>";
                byte[] byteArray = Encoding.UTF8.GetBytes(envelope);
                byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes("itotal" + ":" + "Oracle123");
                string credentials = System.Convert.ToBase64String(toEncodeAsBytes);
                HttpWebRequest request =
                 (HttpWebRequest)WebRequest.Create("https://egqy-test.fa.us6.oraclecloud.com:443/fscmService/ItemServiceV2");
                request.Method = "POST";
                request.ContentType = "text/xml;charset=UTF-8";
                request.ContentLength = byteArray.Length;
                request.Headers.Add("Authorization", "Basic " + credentials);
                request.Headers.Add("SOAPAction", "http://xmlns.oracle.com/apps/scm/productModel/items/fscmService/ItemServiceV2");
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                XDocument doc;
                XmlDocument docu = new XmlDocument();
                string result = "";
                using (WebResponse responseComponent = request.GetResponse())
                {
                    using (Stream stream = responseComponent.GetResponseStream())
                    {
                        doc = XDocument.Load(stream);
                        result = doc.ToString();
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(result);
                        XmlNamespaceManager nms = new XmlNamespaceManager(xmlDoc.NameTable);
                        nms.AddNamespace("env", "http://schemas.xmlsoap.org/soap/envelope/");
                        nms.AddNamespace("wsa", "http://www.w3.org/2005/08/addressing");
                        nms.AddNamespace("typ", "http://xmlns.oracle.com/apps/scm/productModel/items/itemServiceV2/types/");
                        nms.AddNamespace("ns1", "http://xmlns.oracle.com/apps/scm/productModel/items/itemServiceV2/");
                        XmlNodeList nodeList = xmlDoc.SelectNodes("//ns1:ItemCategory", nms);
                        foreach (XmlNode node in nodeList)
                        {
                            ComponentChild component = new ComponentChild();
                            if (node.HasChildNodes)
                            {
                                if (node.LocalName == "ItemCategory")
                                {
                                    XmlNodeList nodeListvalue = node.ChildNodes;
                                    foreach (XmlNode nodeValue in nodeListvalue)
                                    {
                                        if (nodeValue.LocalName == "CategoryName")
                                        {
                                            cats += nodeValue.InnerText + "+";
                                        }
                                    }
                                }
                            }

                        }
                        responseComponent.Close();
                    }
                }

                return cats;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString());
                return "";
            }
        }
        public ComponentChild GetComponentData(ComponentChild component)
        {
            string envelope = "<soapenv:Envelope" +
                                   "   xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"" +
                                   "   xmlns:typ=\"http://xmlns.oracle.com/apps/scm/productModel/items/itemServiceV2/types/\"" +
                                   "   xmlns:typ1=\"http://xmlns.oracle.com/adf/svc/types/\">" +
                                   "<soapenv:Header/>" +
                                   "<soapenv:Body>" +
                                   "<typ:findItem>" +
                                   "<typ:findCriteria>" +
                                   "<typ1:fetchStart>0</typ1:fetchStart>" +
                                   "<typ1:fetchSize>-1</typ1:fetchSize>" +
                                   "<typ1:filter>" +
                                   "<typ1:group>" +
                                   "<typ1:item>" +
                                   "<typ1:conjunction>And</typ1:conjunction>" +
                                   "<typ1:upperCaseCompare>true</typ1:upperCaseCompare>" +
                                   "<typ1:attribute>ItemNumber</typ1:attribute>" +
                                   "<typ1:operator>=</typ1:operator>" +
                                   "<typ1:value>" + component.ItemNumber + "</typ1:value>" +
                                   "</typ1:item>" +
                                   "<typ1:item>" +
                                   "<typ1:conjunction>And</typ1:conjunction>" +
                                   "<typ1:upperCaseCompare>true</typ1:upperCaseCompare>" +
                                   "<typ1:attribute>OrganizationCode</typ1:attribute>" +
                                   "<typ1:operator>=</typ1:operator>" +
                                   "<typ1:value>IO_AEREO_" + component.Airport + "</typ1:value>" +
                                   "</typ1:item>" +
                                   /*  "<typ1:item>" +
                               "<typ1:conjunction>And</typ1:conjunction>" +
                               "<typ1:upperCaseCompare>true</typ1:upperCaseCompare>" +
                               "<typ1:attribute>ItemCategory</typ1:attribute>" +
                               "<typ1:nested>" +
                               "<typ1:group>" +
                               "<typ1:item>" +
                               "<typ1:conjunction>And</typ1:conjunction>" +
                               "<typ1:upperCaseCompare>true</typ1:upperCaseCompare>" +
                               "<typ1:attribute>CategoryName</typ1:attribute>" +
                               "<typ1:operator>=</typ1:operator>" +
                               "<typ1:value>FCC</typ1:value>" +
                               "</typ1:item>" +
                               "</typ1:group>" +
                               "</typ1:nested>" +
                               "</typ1:item>" +*/
                                   "</typ1:group>" +
                                   "</typ1:filter>" +
                                   "<typ1:findAttribute>ItemDescription</typ1:findAttribute>" +
                                   "<typ1:findAttribute>ItemDFF</typ1:findAttribute>" +
                                   "</typ:findCriteria>" +
                                   "<typ:findControl>" +
                                   "<typ1:retrieveAllTranslations>true</typ1:retrieveAllTranslations>" +
                                   "</typ:findControl>" +
                                   "</typ:findItem>" +
                                   "</soapenv:Body>" +
                                   "</soapenv:Envelope>";
            byte[] byteArray = Encoding.UTF8.GetBytes(envelope);
            // Construct the base 64 encoded string used as credentials for the service call
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes("itotal" + ":" + "Oracle123");
            string credentials = System.Convert.ToBase64String(toEncodeAsBytes);
            // Create HttpWebRequest connection to the service
            HttpWebRequest request =
             (HttpWebRequest)WebRequest.Create("https://egqy-test.fa.us6.oraclecloud.com:443/fscmService/ItemServiceV2");
            // Configure the request content type to be xml, HTTP method to be POST, and set the content length
            request.Method = "POST";
            request.ContentType = "text/xml;charset=UTF-8";
            request.ContentLength = byteArray.Length;
            // Configure the request to use basic authentication, with base64 encoded user name and password, to invoke the service.
            request.Headers.Add("Authorization", "Basic " + credentials);
            // Set the SOAP action to be invoked; while the call works without this, the value is expected to be set based as per standards
            request.Headers.Add("SOAPAction", "http://xmlns.oracle.com/apps/scm/productModel/items/itemServiceV2/findItem");
            // Write the xml payload to the request
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            // Write the xml payload to the request
            XDocument doc;
            XmlDocument docu = new XmlDocument();
            string result = "";
            // Get the response and process it; In this example, we simply print out the response XDocument doc;
            using (WebResponse responseComponentGet = request.GetResponse())
            {
                using (Stream stream = responseComponentGet.GetResponseStream())
                {
                    doc = XDocument.Load(stream);
                    result = doc.ToString();
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(result);
                    XmlNamespaceManager nms = new XmlNamespaceManager(xmlDoc.NameTable);
                    nms.AddNamespace("env", "http://schemas.xmlsoap.org/soap/envelope/");
                    nms.AddNamespace("wsa", "http://www.w3.org/2005/08/addressing");
                    nms.AddNamespace("typ", "http://xmlns.oracle.com/apps/scm/productModel/items/itemServiceV2/types/");
                    nms.AddNamespace("ns0", "http://xmlns.oracle.com/adf/svc/types/");
                    nms.AddNamespace("ns1", "http://xmlns.oracle.com/apps/scm/productModel/items/itemServiceV2/");

                    XmlNodeList nodeList = xmlDoc.SelectNodes("//ns0:Value", nms);
                    foreach (XmlNode node in nodeList)
                    {
                        if (node.HasChildNodes)
                        {
                            if (node.LocalName == "Value")
                            {
                                XmlNodeList nodeListvalue = node.ChildNodes;
                                foreach (XmlNode nodeValue in nodeListvalue)
                                {
                                    if (nodeValue.LocalName == "ItemDescription")
                                    {
                                        component.ItemDescription = nodeValue.InnerText.Trim().Replace("/", "");
                                    }
                                    if (nodeValue.LocalName == "ItemDFF")
                                    {
                                        XmlNodeList nodeListDeff = nodeValue.ChildNodes;
                                        {
                                            foreach (XmlNode nodeDeff in nodeListDeff)
                                            {
                                                if (nodeDeff.LocalName == "xxParticipacionCobro")
                                                {
                                                    component.ParticipacionCobro = nodeDeff.InnerText == "SI" ? "1" : "0";
                                                }
                                                if (nodeDeff.LocalName == "xxCobroParticipacionNj")
                                                {
                                                    component.CobroParticipacionNj = nodeDeff.InnerText;
                                                }
                                                if (nodeDeff.LocalName == "xxPagos")
                                                {
                                                    component.Pagos = nodeDeff.InnerText;
                                                }
                                                if (nodeDeff.LocalName == "xxClasificacionPago")
                                                {
                                                    component.ClasificacionPagos = nodeDeff.InnerText;
                                                }
                                                if (nodeDeff.LocalName == "cuentaGastoCx")
                                                {
                                                    component.CuentaGasto = nodeDeff.InnerText;
                                                }
                                                if (nodeDeff.LocalName == "xxInformativo")
                                                {
                                                    component.Informativo = nodeDeff.InnerText == "SI" ? "1" : "0";
                                                }
                                                if (nodeDeff.LocalName == "xxPaqueteInv")
                                                {
                                                    component.Paquete = nodeDeff.InnerText == "SI" ? "1" : "0";

                                                }
                                            }
                                        }

                                    }

                                }
                            }
                        }
                    }

                }
                responseComponentGet.Close();
            }
            return component;

        }
        public void InsertComponent(ComponentChild component)
        {
            try
            {
                var client = new RestClient("https://iccsmx.custhelp.com/");
                var request = new RestRequest("/services/rest/connect/v1.4/CO.Services/", Method.POST);
                request.RequestFormat = DataFormat.Json;
                string body = "{";
                // Información de precios costos
                body += "\"Airport\":\"" + component.Airport + "\",";
                if (String.IsNullOrEmpty(component.CobroParticipacionNj))
                {
                    body += "\"CobroParticipacionNj\":null,";
                }
                else
                {
                    body += "\"CobroParticipacionNj\":\"" + component.CobroParticipacionNj + "\",";
                }
                if (String.IsNullOrEmpty(component.ClasificacionPagos))
                {
                    body += "\"ClasificacionPagos\":null,";
                }
                else
                {
                    body += "\"ClasificacionPagos\":\"" + component.ClasificacionPagos + "\",";
                }
                body += "\"Componente\":\"" + component.Componente + "\",";

                if (String.IsNullOrEmpty(component.Costo))
                {
                    body += "\"Costo\":null,";
                }
                else
                {
                    body += "\"Costo\":\"" + component.Costo + "\",";
                }
                body += "\"Incident\":";
                body += "{";
                body += "\"id\":" + Convert.ToInt32(component.Incident) + "";
                body += "},";
                body += "\"Informativo\":\"" + component.Informativo + "\"," +
                 "\"ItemDescription\":\"" + component.ItemDescription + "\"," +
                 "\"ItemNumber\":\"" + component.ItemNumber + "\",";
                if (IdItinerary != 0)
                {
                    body += "\"Itinerary\":";
                    body += "{";
                    body += "\"id\":" + IdItinerary + "";
                    body += "},";
                }
                if (String.IsNullOrEmpty(component.Pagos))
                {
                    body += "\"Pagos\":null,";
                }
                else
                {
                    body += "\"Pagos\":\"" + component.Pagos + "\",";
                }
                body += "\"Paquete\":\"" + component.Paquete + "\",";
                if (String.IsNullOrEmpty(component.ParticipacionCobro))
                {
                    body += "\"ParticipacionCobro\":null,";
                }
                else
                {
                    body += "\"ParticipacionCobro\":\"" + component.ParticipacionCobro + "\",";
                }
                if (String.IsNullOrEmpty(component.Precio))
                {
                    body += "\"Precio\":null";
                }
                else
                {
                    body += "\"Precio\":\"" + component.Precio + "\"";
                }
                body += "}";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                // easily add HTTP Headers
                request.AddHeader("Authorization", "Basic ZW9saXZhczpTaW5lcmd5KjIwMTg=");
                request.AddHeader("X-HTTP-Method-Override", "POST");
                request.AddHeader("OSvC-CREST-Application-Context", "Create Service");
                // execute the request
                IRestResponse response = client.Execute(request);
                var content = response.Content; // raw content as string
                RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(response.Content);
                if (response.StatusCode == HttpStatusCode.Created)
                {

                    InsertedServices.Add(rootObject);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en creación de child: " + ex.Message);
            }

        }
        private void CboProductos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dataGridServicios.DataSource = null;
            List<Items> s = new List<Items>();
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                foreach (Items i in items)
                {
                    if (i.Description.Contains(txtSearch.Text.ToUpper()))
                    {
                        s.Add(i);
                    }
                }
            }
            else
            {
                s = items;
            }
            dataGridServicios.DataSource = s;
            dataGridServicios.Columns[0].Visible = false;
            dataGridServicios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void dataGridServicios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ItiObject != null || Incident != null)
            {
                ItemNumberParent = dataGridServicios.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
                ItemDesc = dataGridServicios.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();
                GetPData(AirtportText, SRType, ItemNumberParent);
            }
        }
    }
    public class Items
    {
        public string ItemNumber { get; set; }
        public string Description { get; set; }
    }
    public class ComponentChild
    {
        public string Airport
        { get; set; }
        public string CobroParticipacionNj
        { get; set; }
        public string ClasificacionPagos
        { get; set; }
        public string Componente
        { get; set; }
        public string Costo
        { get; set; }
        public string CuentaGasto
        { get; set; }
        public int Incident
        { get; set; }
        public string Informativo
        { get; set; }
        public string ItemDescription
        { get; set; }
        public string ItemNumber
        { get; set; }
        public int Itinerary
        { get; set; }
        public string Pagos
        { get; set; }
        public string Paquete
        { get; set; }
        public string ParticipacionCobro
        { get; set; }
        public string Precio
        { get; set; }
    }
}