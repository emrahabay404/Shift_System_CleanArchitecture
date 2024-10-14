namespace Shift_System.Shared.Helpers
{
    public static class Messages
    {
        public const string File_Not_Found_TR = "Dosya bulunamadı.";
        public const string File_Size_Exceeded_TR = "Dosya boyutu aşıldı.";
        public const string Invalid_File_Type_TR = "Geçersiz dosya türü.";
        public const string Invalid_File_Type_OR_Size_TR = "Geçersiz dosya türü veya boyutu.";
        public const string File_Not_Save_TR = "Dosya kaydedilemedi.";
        public const string File_Upload_Success_TR = "Dosya başarıyla yüklendi.";
        // Role creation
        public static string Role_Created_Successfully_TR => "Rol başarıyla oluşturuldu.";
        public static string Role_Created_Successfully_EN => "Role created successfully.";
        public static string Role_Already_Exists_TR => "Bu isimde bir rol zaten mevcut.";
        public static string Role_Already_Exists_EN => "A role with this name already exists.";
        public static string Role_Creation_Failed_TR => "Rol oluşturma başarısız.";
        public static string Role_Creation_Failed_EN => "Role creation failed.";

        // File Upload Specific Messages 
        public static string File_Upload_Failed_TR => "Dosya yükleme başarısız oldu.";
        public static string File_Deleted_Success_TR => "Dosya başarıyla silindi.";
        public static string No_Files_Uploaded_TR => "Hiçbir dosya yüklenemedi.";
        public static string All_Files_Uploaded_Success_TR => "Tüm dosyalar başarıyla yüklendi.";
        public static string Some_Files_Failed_TR => "Bazı dosyalar yüklenemedi.";

        // Auth
        public static string Login_Failed_TR => "Kullanıcı | Şifre Yanlış";
        public static string Login_Failed_EN => "User | Password Is Wrong";
        public static string Token_Created_Success_TR => "Jeton Başarıyla Oluşturuldu";
        public static string Token_Created_Success_EN => "Token Has Been Successfully Created";

        public static string User_Registered_Successfully_TR => "Kullanıcı başarıyla kaydedildi.";
        public static string User_Registered_Successfully_EN => "User registered successfully.";
        public static string User_Registration_Failed_TR => "Kullanıcı kaydı başarısız.";
        public static string User_Registration_Failed_EN => "User registration failed.";

        // Role assignment
        public static string Role_Assigned_Successfully_TR => "Rol başarıyla kullanıcıya atandı.";
        public static string Role_Assignment_Failed_TR => "Rol atama işlemi başarısız.";
        public static string Role_Not_Found_TR => "Rol bulunamadı.";

        // User retrieval
        public static string User_Not_Found_TR => "Kullanıcı bulunamadı.";

        // Validation
        public static string Invalid_Input_TR => "Geçersiz giriş.";
        public static string Invalid_Input_EN => "Invalid input.";
        public static string Required_Field_Missing_TR => "Gerekli alan eksik.";
        public static string Required_Field_Missing_EN => "Required field is missing.";

        // General errors
        public static string Unexpected_Error_TR => "Beklenmeyen bir hata oluştu.";
        public static string Unexpected_Error_EN => "An unexpected error occurred.";

        // Yeni eklenen değerler
        public static string Token_Could_Not_Be_Received_TR => "Jeton Alınamadı";
        public static string Token_Could_Not_Be_Received_EN => "Token Could Not Be Received";

        public static string Jwt_Token_Storage_Failed_TR => "Jwt Jetonu Saklama Başarısız Oldu";
        public static string Jwt_Token_Storage_Failed_EN => "Jwt Token Storage Failed";

        // Teams
        public static string Teams_Listed_TR => "Takımlar Listelendi";
        public static string Teams_Listed_EN => "Teams Listed";

        // Data Operations
        public static string Data_Saved_Successfully_TR => "Veri başarıyla kaydedildi.";
        public static string Data_Saved_Successfully_EN => "Data saved successfully.";
        public static string Data_Deletion_Confirmed_TR => "Veri silme işlemi onaylandı.";
        public static string Data_Deletion_Confirmed_EN => "Data deletion confirmed.";

        // General Errors
        public static string Operation_Failed_TR => "İşlem başarısız oldu.";
        public static string Operation_Failed_EN => "Operation failed.";

        public static string Operation_Success_TR => "İşlem başarıyla tamamlandı.";
        public static string Operation_Success_EN => "Operation successful.";

        // Notifications
        public static string New_Message_Notification_TR => "Yeni Mesaj Bildirimi";
        public static string New_Message_Notification_EN => "New Message Notification";

        // Web API Specific Messages
        public static string Api_Unauthorized_Access_TR => "Yetkisiz Erişim";
        public static string Api_Unauthorized_Access_EN => "Unauthorized Access";

        public static string Api_Resource_Not_Found_TR => "Kaynak Bulunamadı";
        public static string Api_Resource_Not_Found_EN => "Resource Not Found";

        public static string Api_Bad_Request_TR => "Geçersiz İstek";
        public static string Api_Bad_Request_EN => "Bad Request";

        public static string Api_Internal_Server_Error_TR => "Sunucu hatası oluştu";
        public static string Api_Internal_Server_Error_EN => "Internal server error";

        public static string Api_Forbidden_TR => "Erişim reddedildi.";
        public static string Api_Forbidden_EN => "Access denied.";

        // Web Application Specific Messages
        public static string Session_Expired_TR => "Oturum süresi doldu.";
        public static string Session_Expired_EN => "Session expired.";

        public static string Page_Not_Found_TR => "Sayfa bulunamadı.";
        public static string Page_Not_Found_EN => "Page not found.";

        public static string Form_Submission_Success_TR => "Form başarıyla gönderildi.";
        public static string Form_Submission_Success_EN => "Form submitted successfully.";

        public static string Form_Submission_Failure_TR => "Form gönderilirken bir hata oluştu.";
        public static string Form_Submission_Failure_EN => "An error occurred while submitting the form.";

        public static string Logout_Successful_TR => "Çıkış başarılı.";
        public static string Logout_Successful_EN => "Logout successful.";

        public static string Permission_Denied_TR => "Bu işlemi gerçekleştirme izniniz yok.";
        public static string Permission_Denied_EN => "You do not have permission to perform this action.";

        // Additional Popular Operations

        // Password Reset
        public static string Password_Reset_Success_TR => "Şifre başarıyla sıfırlandı.";
        public static string Password_Reset_Success_EN => "Password reset successfully.";

        public static string Password_Reset_Failed_TR => "Şifre sıfırlama başarısız oldu.";
        public static string Password_Reset_Failed_EN => "Password reset failed.";

        // Account Activation
        public static string Account_Activation_Success_TR => "Hesap başarıyla aktifleştirildi.";
        public static string Account_Activation_Success_EN => "Account activated successfully.";

        public static string Account_Activation_Failed_TR => "Hesap aktivasyonu başarısız oldu.";
        public static string Account_Activation_Failed_EN => "Account activation failed.";

        // File Upload 
        public static string File_Upload_Success_EN => "File uploaded successfully.";

        public static string File_Upload_Failed_EN => "File upload failed."; 

        // Email Sending
        public static string Email_Sent_Success_TR => "E-posta başarıyla gönderildi.";
        public static string Email_Sent_Success_EN => "Email sent successfully.";

        public static string Email_Sent_Failed_TR => "E-posta gönderimi başarısız oldu.";
        public static string Email_Sent_Failed_EN => "Email sending failed.";
    }
}
