
//namespace Shift_System.Shared.Interfaces
//{
//   public interface IResult<T>
//   {
//      List<string> Messages { get; set; }
//      bool Succeeded { get; set; }
//      T Data { get; set; }
//      //List<ValidationResult> ValidationErrors { get; set; }
//      Exception Exception { get; set; }
//      int Code { get; set; }
//   }
//}


namespace Shift_System.Shared.Interfaces
{
    public interface IResult<T>
    {
        string Message { get; set; } // Liste yerine tek bir mesaj string olarak kullanılıyor
        bool Succeeded { get; set; }
        T Data { get; set; }
        Exception Exception { get; set; }
        int Code { get; set; }
    }
}
