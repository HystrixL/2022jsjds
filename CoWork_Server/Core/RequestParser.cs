using System;
using System.Text.RegularExpressions;
using Co_Work.Network;
using Co_Work.Network.TCP;

namespace Co_Work.Core
{
    public class RequestParser
    {
        private string _request;
        public Type RequestType = null;
        public RequestParser(string request)
        {
            this._request = request;
        }

        private string GetRequestType()
        {
            var regex = new Regex("[a-zA-Z]+:{");
            var rawTypeString = regex.Match(_request).Value;
            var typeString = rawTypeString.Replace(":{", "");

            return typeString;
        }

        private string GetRequestBody()
        {
            var typeString = GetRequestType();
            return _request.Replace(typeString+":", "");
        }

        public void Parse(Client client)
        {
            var requestType = GetRequestType();
            var requestBody = GetRequestBody();

            switch (requestType)
            {
                case nameof(Request.Login):
                    Request.Login.Parse(TransData<Request.Login>.Convert(requestBody),client);
                    break;
                case nameof(Request.Register):
                    Request.Register.Parse(TransData<Request.Register>.Convert(requestBody),client);
                    break;
                case nameof(Request.GetFileInfo):
                    Request.GetFileInfo.Parse(TransData<Request.GetFileInfo>.Convert(requestBody),client);
                    break;
                case nameof(Request.CreatProject):
                    Request.CreatProject.Parse(TransData<Request.CreatProject>.Convert(requestBody),client);
                    break;
                case nameof(Request.UpdateProject):
                    Request.UpdateProject.Parse(TransData<Request.UpdateProject>.Convert(requestBody),client);
                    break;
                case nameof(Request.DeleteProject):
                    Request.DeleteProject.Parse(TransData<Request.DeleteProject>.Convert(requestBody),client);
                    break;
                case nameof(Request.GetEmployeesInfo):
                    Request.GetEmployeesInfo.Parse(TransData<Request.GetEmployeesInfo>.Convert(requestBody),client);
                    break;
                case nameof(Request.GetEmployeeInfoFromGuid):
                    Request.GetEmployeeInfoFromGuid.Parse(TransData<Request.GetEmployeeInfoFromGuid>.Convert(requestBody),client);
                    break;
                case nameof(Request.GetEmployeeInfoFromId):
                    Request.GetEmployeeInfoFromId.Parse(TransData<Request.GetEmployeeInfoFromId>.Convert(requestBody),client);
                    break;
                case nameof(Request.GetProjectsInfoFromEmployee):
                    Request.GetProjectsInfoFromEmployee.Parse(TransData<Request.GetProjectsInfoFromEmployee>.Convert(requestBody),client);
                    break;
                case nameof(Request.GetProjectsInfo):
                    Request.GetProjectsInfo.Parse(TransData<Request.GetProjectsInfo>.Convert(requestBody),client);
                    break;
                case nameof(Request.GetProjectInfo):
                    Request.GetProjectInfo.Parse(TransData<Request.GetProjectInfo>.Convert(requestBody),client);
                    break;
                default:
                    break;
            }
        }
    }
}