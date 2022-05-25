using Newtonsoft.Json;
using System.Collections.Generic;
namespace protecta.laft.api.DTO
{
    public class ResourceProfileParametersDTO
    {
        public int nIdProfile { get; set; }
        public int nIdUser { get; set; }
        public string items { get; set; }
        public List<Data2> items2 { get; set; }
        //{
        //    get 
        //    { 
        //        return items2; 
        //    };
        //    set
        //    {
        //        JsonConvert.DeserializeObject<List<Data2>>(items);
        //    }
        //}
    }
    public class Data2
    {
        public bool isChecked {get;set;}
        public string sProfileName { get; set; }
        public int nIdResource { get; set; }
        public int nIdProfile { get; set; }
        public string nResourceName { get; set; }
        public string sMenu { get; set; }
        public string sSubMenu { get; set; }
        public int nIdFather {get;set;}
    }
}