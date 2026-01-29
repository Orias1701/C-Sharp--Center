using System.Reflection;

namespace WarehouseManagement.Helpers
{
    /// <summary>
    /// Lấy phiên bản ứng dụng từ assembly (trùng với Version/FileVersion trong .csproj).
    /// Khi release, chỉ cần đổi Version trong .csproj là footer và thuộc tính file EXE đều cập nhật.
    /// </summary>
    public static class AppVersion
    {
        private static readonly string _version = GetVersionFromAssembly();

        /// <summary>
        /// Chuỗi phiên bản hiển thị, ví dụ: "1.0.1"
        /// </summary>
        public static string Current => _version;

        /// <summary>
        /// Chuỗi hiển thị có tiền tố "v", ví dụ: "v1.0.1"
        /// </summary>
        public static string Display => "v" + _version;

        private static string GetVersionFromAssembly()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var infoVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
                if (!string.IsNullOrEmpty(infoVersion?.InformationalVersion))
                {
                    // Bỏ build metadata (phần sau dấu +) để footer chỉ hiển thị "v1.0.1"
                    string full = infoVersion.InformationalVersion;
                    int plus = full.IndexOf('+');
                    return plus >= 0 ? full.Substring(0, plus).Trim() : full;
                }

                var version = assembly.GetName().Version;
                if (version != null)
                    return $"{version.Major}.{version.Minor}.{version.Build}";
            }
            catch
            {
                // ignore
            }

            return "1.0.0";
        }
    }
}
