using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickBuy_Api.Database.Enums
{
    public enum MenuAppRoleType
    {
        SystemDataView = 1,
        SystemDataEdit = 2,
        SystemDataDelete = 3,
        PersonalDataView = 4,
        PersonalDataEdit = 5,
        PersonalDataDelete = 6,
        DownloadExcel = 7
    }
}
