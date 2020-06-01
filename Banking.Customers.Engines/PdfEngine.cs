using System.IO;
using Banking.Customers.Domain.Extensions;
using Banking.Customers.Services;
using Wkhtmltopdf.NetCore;

namespace Banking.Customers.Engines
{
    public interface IPdfEngine
    {
        byte[] GeneratePdf(string url);
    }


    public class PdfEngine : IPdfEngine
    {
        private readonly IImageService _imageService;
        private readonly IGeneratePdf _pdfService;


        public PdfEngine(IImageService imageService, IGeneratePdf generatePdf)
        {
            _imageService = imageService;
            _pdfService = generatePdf;
        }

        

        public byte[] GeneratePdf(string url)
        {
            var image = _imageService.GetImage(url);
            var htmlContent = GetImageHtml(image);

            return _pdfService.GetPDF(htmlContent);

        }

        
        private string GetImageHtml(Stream image)
        {
            string imageBase64String = image.ConvertToBytes().ToStringBase64();

            string html = $@"<html style='height:100%;width:100%;margin:0;padding:0;'>
             <head></head>
             <body style='height:100%;width:100%;margin:0;padding:0;
                background:url(data:image/png;base64,{imageBase64String}) 
                no-repeat center center fixed;-webkit-background-size:cover;-o-background-size:cover;
                -moz-background-size:cover;background-size:cover;'>
             </body>
             </html>";
            return html;

        }

        private string GetImagesHtml(Stream image)
        {
            string imageBase64String = image.ConvertToBytes().ToStringBase64();

            string html = $@"<html style='height:100%;width:100%;margin:0;padding:0;'>
             <head></head>
             <body>
                <img src='data:image/png;base64,{imageBase64String}' />
             </body>
             </html>";
            return html;

        }
    }
}
