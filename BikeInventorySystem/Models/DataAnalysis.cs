using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BikeInventorySystem.Models

{
    /* DataAnalysisS Method*/
    public static class DataAnalysisS
    {
        public static List<DataAnalysisDTO> DataAnalysisDTO(Guid userId)
        {
            List<DataAnalysisDTO> dataAnalysisDTO = new List<DataAnalysisDTO>();
            var data = ApprovedItemService.GetAll(userId);
            foreach (var item in data)
            {
                dataAnalysisDTO.Add(new DataAnalysisDTO { TitleName = item.ItemName, ValueCount = item.Quantity });
            }
            return dataAnalysisDTO;
        }
    }

    /* DataAnalysisDTO Method*/
    public class DataAnalysisDTO
    {
        public string TitleName { get; set; }
        public int ValueCount { get; set; }
    }

}
