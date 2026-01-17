namespace WarehouseManagement.Views.Panels
{
    /// <summary>
    /// Interface để các panel có thể thực hiện tìm kiếm thông qua thanh công cụ chính
    /// </summary>
    public interface ISearchable
    {
        void Search(string searchText);
    }
}
