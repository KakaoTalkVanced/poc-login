using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System.Windows;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;

namespace login;

public partial class MainWindow : Window
{
    readonly Uri _baseAddress = new Uri("https://katalk.kakao.com");
    readonly string _loginEndPoint = "win32/account/login.json";
    readonly string _xvcSource = "HEATH|KT/3.1.4 Wd/10.0 ko|DEMIAN|abc@naver.com|OMnpb2Rq6q4goIvDM/yiHxs7ztsaGnNtjdXmFW92SODvof2BwjvJIwbP5cDp4b++fcYCBGQYy6K8Q8jGhZYzV1==";

    public MainWindow()
    {
        InitializeComponent();
    }

    void Button_Click(object sender, RoutedEventArgs e)
    {
        Request();
    }

    void Request()
    {
        using StringContent jsonContent = new(
        JsonSerializer.Serialize(new
        {
            email = "test@test.com",
            password = "password",
            device_uuid = "OMnpb2Rq6q4goIvDM/yiHxs7ztsaGnNtjdXmFW92SODvof2BwjvJIwbP5cDp4b++fcYCBGQYy6K8Q8jGhZYzV1==",
            os_version = "3.2.8",
            device_name = "TEST_DEVICE",
            permanent = true,
            forced = true
        }),
        Encoding.UTF8,
        "application/json");

        using var httpClient = new HttpClient()
        {
            BaseAddress = _baseAddress,
        };
        var xvcBytes = Encoding.UTF8.GetBytes(_xvcSource);
        var hash = new SHA512Managed().ComputeHash(xvcBytes);
        var fullKey = hash[0..16];
        var xvcKey = Encoding.ASCII.GetString(fullKey);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("X-VC", xvcKey);

        var requestTask = httpClient.PostAsJsonAsync(_loginEndPoint, jsonContent);

        var httpResponseMessage = requestTask.Result;

        ResultTextBlock.Text = httpResponseMessage.Content.ReadAsStringAsync().Result;
    }
}
