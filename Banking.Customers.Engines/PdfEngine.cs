using System;
using System.Collections.Generic;
using System.Dynamic;
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




    public class DynamicDictionary : DynamicObject
    {
        // The inner dictionary.
        Dictionary<string, object> dictionary= new Dictionary<string, object>();

        public DynamicDictionary()
        {

        }


        public DynamicDictionary(List<Field> fields)
        {

            //fields.ForEach(x => dictionary.Add(x.FieldName,x.FielValue));
            fields.ForEach(x => dictionary[x.FieldName] = x.FielValue);
        }


        // This property returns the number of elements
        // in the inner dictionary.
        public int Count
        {
            get
            {
                return dictionary.Count;
            }
        }

        // If you try to get a value of a property
        // not defined in the class, this method is called.
        public override bool TryGetMember( GetMemberBinder binder, out object result)
        {
            // Converting the property name to lowercase
            // so that property names become case-insensitive.
            string name = binder.Name.ToLower();

            // If the property name is found in a dictionary,
            // set the result parameter to the property value and return true.
            // Otherwise, return false.
            return dictionary.TryGetValue(name, out result);
        }

        // If you try to set a value of a property that is
        // not defined in the class, this method is called.
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            // Converting the property name to lowercase
            // so that property names become case-insensitive.
            dictionary[binder.Name.ToLower()] = value;

            // You can always add a value to a dictionary,
            // so this method always returns true.
            return true;
        }


        public ExpandoObject CreateDynamicCustomer(string Name)
        {
            dynamic cust = new ExpandoObject();
            cust.FullName = Name;
            cust.ChangeName = (Action<string>)((string newName) =>
            {
                cust.FullName = newName;
            });
            return cust;
        }

        public ExpandoObject CreateDynamicCustomer(string propertyName, string PropertyValue)
        {
            dynamic cust = new ExpandoObject();
            ((IDictionary<string, object>)cust)[propertyName] = PropertyValue;
            return cust;
        }

        

    }


    public class DynamicClass : DynamicObject
    {
        private Dictionary<string, KeyValuePair<Type, object>> _fields;

        public DynamicClass(List<Field> fields)
        {
            _fields = new Dictionary<string, KeyValuePair<Type, object>>();
            fields.ForEach(x => _fields.Add(x.FieldName,
                new KeyValuePair<Type, object>(x.FieldType, null)));
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (_fields.ContainsKey(binder.Name))
            {
                var type = _fields[binder.Name].Key;
                if (value.GetType() == type)
                {
                    _fields[binder.Name] = new KeyValuePair<Type, object>(type, value);
                    return true;
                }
                else throw new Exception("Value " + value + " is not of type " + type.Name);
            }
            return false;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _fields[binder.Name].Value;
            return true;
        }
    }


    public class Field
    {
        public Field(string name, Type type)
        {
            this.FieldName = name;
            this.FieldType = type;
        }

        public Field(string name, string value)
        {
            this.FieldName = name;
            this.FielValue = value;
        }

        public string FieldName;
        public string FielValue;
        public Type FieldType;
    }
}
