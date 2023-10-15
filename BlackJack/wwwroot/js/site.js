// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

    $(document).ready(function () {
        $("#deal-cards-button").click(function () {
            $.ajax({
                url: '/Game/DealCards', // Controller ve Action belirtilir
                type: 'POST', // HTTP isteği türü (GET veya POST)
                success: function (result) {
                    // İşlem başarılı olduğunda yapılacak işlemler
                    // Örneğin, sayfayı yeniden yükleme (location.reload()) veya sonuçları görüntüleme
                },
                error: function (error) {
                    // İşlem sırasında hata oluştuğunda yapılacak işlemler
                    // Hata mesajlarını görüntüleme veya kullanıcıya bilgi verme
                }
            });
        });
        });
