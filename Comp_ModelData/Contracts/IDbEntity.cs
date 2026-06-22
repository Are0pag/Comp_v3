namespace Comp.ModelData.Contracts;

public interface IDbEntity
{
    public int Id { get; set; }
    
    /// <summary>
    /// Возвращает копию объекта с теми же значениями полей
    /// </summary>
    IDbEntity Clone(); 
}