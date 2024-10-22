using Hackathon.Common.Models.Base;

namespace Hackathon.Common.Models;

/// <summary>
/// Параметры для получения списка элементов
/// <remarks>
/// Фильтр, с пагинацией и сортировкой
/// </remarks>
/// </summary>
/// <typeparam name="T"></typeparam>
public class GetListParameters<T>: PaginationSort where T: class
{
    public T Filter { get; set; }
}
