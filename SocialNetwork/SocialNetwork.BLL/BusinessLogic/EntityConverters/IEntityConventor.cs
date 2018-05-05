using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.BLL.BusinessLogic.EntityConverters
{
    /// <summary>
    /// Declares methods for convertation BLL entities to original
    /// </summary>
    /// <typeparam name="T">BLL entity</typeparam>
    /// <typeparam name="Y">Original entity</typeparam>
    internal interface IEntityConverter<T, Y>
    {
        /// <summary>
        /// Converts original entity to the BLL entity
        /// </summary>
        /// <param name="originalEntity"></param>
        /// <returns>Converted BLL entity</returns>
        T ConvertToBLLEntity(Y originalEntity);

        /// <summary>
        /// Converts BLL entity to the Original entity
        /// </summary>
        /// <param name="BLL Entiry"></param>
        /// <returns>Converted Original entity</returns>
        Y ConvertToOriginalEntity(T bllEntity);
    }
}
