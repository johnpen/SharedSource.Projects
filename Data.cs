using Sitecore.Data;

namespace SharedSource.Projects
{
    /// <summary>
    /// ID's of projects items
    /// </summary>
    public static class Data
    {
        public static ID ProjectTemplate = new ID("{ABBCB00E-7FDD-41A4-AE30-0BF8B0CE967D}");
        public static ID ProjectFieldId = new ID("{9E26F08E-5510-4E45-BC98-3B1C31F7D89F}");
        public static ID ProjectWorkflow = new ID("{500AD4A2-A65B-4116-8D5E-BDBBCA31FFD6}");
        public static ID ProjectFolder = new ID("{D2A33F8C-4C43-48F5-BDAF-A68395831F9E}");
        public static ID ProjectDetails = new ID("{E0A3B8FB-D5D4-4EA9-B4BC-EA8B41F541C6}");

        public static ID ProjectDetailsName = new ID("{AE985D54-1455-4095-9CC1-B5FB8BA7F795}");
        public static ID ProjectDetailsDescription = new ID("{B19AE1FF-B976-4A4B-B58E-E5C1C7BA7E63}");
        public static ID ProjectDetailsReleaseDate = new ID("{88DCB148-BA21-440A-9C33-D94BFEE1B788}");

        public static ID ProjectWorkflowReadytoGoLiveState = new ID("{4EC74031-9ADB-44B0-BA25-14F311B2FD6B}");
        public static ID GlobalProductTemplate = new ID("{E2B7DE50-4B5B-4362-8722-8218637FA78D}");
        public static ID GlobalproductDefaultWorkflow = new ID("{E975D3BD-3CE7-4C1F-8588-4FA6243F879D}");


        public static string ProjectRootFolder = "/sitecore/content/Settings/Projects";
    }
}
