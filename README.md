# filmAPI

Proje ayağa kalkarken Data/xxx.json dosyaları oluşmaktadır. SeedData mantığı var. Her bir json dosyası table olarak görülebilir. Domain üzerinde modellerimiz mevcut.

Authorization kısmı JWT sayesinde çalışıyor. Default olarak JWT token'ın ömrü gayet uzun ve refresh token mantığı projenin boyutundan dolayı uygulanmadı. Projeyi çalıştırıp swagger ekranına gelindiğinde bir kullanıcı register edip o kullanıcı olarak giriş yapabilirsiniz. Giriş yaptığınızda size JWT token'ı verilip o token'ı sağ tepedeki "authorize" tuşuna basarak açılan diyaloğun içine yerleştirip "login" diyebilirsiniz. (Başına 'bearer' yazılmasına gerek yok, default olarak yazılacaktır).

Film controllerındaki bütün endpointler test amaçlı loginli olmayı gerektirmektedir.

Bütün veriler .json lar içerisinde tutulmakta ve bu dosyarların var olmadığı durumlarda sistem otomatik olarak oluşturacaktır.

"DataStore" adlı classımız genel olarak generic type lar ile çalışmak için oluşturulmuştur. Dolayısıyla oluşturulan herhangi yeni .json (json dosyasını database te bir table olarak düşünebiliriz) birkaç yeni satır eklenerek çalışır hale getirilebilir. 

BaseClass id yi tutup ulaşmak için kullanılmaktadır. GUID ile random id oluşmaktadır.

Log-lama mantığı tekrardan .json içerisinde tutulmakta