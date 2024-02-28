using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;

namespace login;

public partial class MainWindow : Window
{
    readonly string _baseUrl = "https://katalk.kakao.com";

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
        using var client = new HttpClient();

        var requestTask = client.PostAsJsonAsync(_baseUrl, string.Empty);

        var httpResponseMessage = requestTask.Result;

        ResultTextBlock.Text = httpResponseMessage.ToString();
    }
}
