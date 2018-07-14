using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web.Services.Description;
using Microsoft.CSharp;

namespace hwj.CommonLibrary.Object
{
    public class WebServiceHelper
    {
        [Serializable]
        public class InvokeEntity
        {
            #region Property
            public object iObject { get; set; }
            public MethodInfo iMethodInfo { get; set; }
            public Type Type { get; set; }
            public string MethodName { get; set; }
            #endregion

            public InvokeEntity() { }
            public InvokeEntity(string fileName, string url, string classname, int timeout)
                : this(fileName, url, classname, null, timeout)
            {
            }

            public InvokeEntity(string fileName, string url, string classname, string methodname, int timeout)
            {
                this.MethodName = methodname;

                if ((classname == null) || (classname == ""))
                {
                    classname = GetWsClassName(url);
                }
                Assembly assembly = Assembly.LoadFrom(fileName);
                Type t = assembly.GetType(@namespace + "." + classname, true, true);

                object obj = Activator.CreateInstance(t);

                if (timeout != -1)
                {
                    PropertyInfo propInfo = obj.GetType().GetProperty("Timeout");
                    propInfo.SetValue(obj, timeout, null);
                }

                this.iObject = obj;
                this.Type = t;

                if (!string.IsNullOrEmpty(methodname))
                {
                    this.iMethodInfo = t.GetMethod(methodname);
                }
            }

            public object Invoke(string methodname, object[] args)
            {
                return InvokeWebServiceByDLL(this, methodname, args);
            }
        }

        #region InvokeWebService
        private const string @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
        //动态调用web服务
        public static object InvokeWebService(string url, string methodname, object[] args)
        {
            return WebServiceHelper.InvokeWebService(url, null, methodname, args);
        }
        public static object InvokeWebService(string url, string classname, string methodname, object[] args)
        {
            return InvokeWebService(url, classname, methodname, args, null);
        }
        public static object InvokeWebServiceByDLL(string fileName, string url, string classname, string methodname, object[] args)
        {
            return InvokeWebServiceByDLL(fileName, url, classname, methodname, -1, args);
        }
        //public static object InvokeWebServiceByDLL(string fileName, string url, string classname, string methodname, int timeout, object[] args)
        //{
        //    //try
        //    //{
        //    if ((classname == null) || (classname == ""))
        //    {
        //        classname = GetWsClassName(url);
        //    }
        //    Assembly assembly = Assembly.LoadFrom(fileName);
        //    Type t = assembly.GetType(@namespace + "." + classname, true, true);

        //    object obj = Activator.CreateInstance(t);
        //    MethodInfo mi = t.GetMethod(methodname);

        //    if (timeout != -1)
        //    {
        //        PropertyInfo propInfo = obj.GetType().GetProperty("Timeout");
        //        propInfo.SetValue(obj, timeout, null);
        //    }


        //    if (mi != null)
        //    {
        //        if (args == null)
        //            return mi.Invoke(obj, null);
        //        else
        //        {
        //            ParameterInfo[] paramsInfo = mi.GetParameters();
        //            object[] tmpArgs = new object[args.Length];

        //            for (int i = 0; i < args.Length; i++)
        //            {
        //                Type tType = paramsInfo[i].ParameterType;
        //                //如果它是值类型,或者String   
        //                if (tType.Equals(typeof(string)) || (!tType.IsInterface && !tType.IsClass))
        //                {
        //                    //改变参数类型   
        //                    tmpArgs[i] = Convert.ChangeType(args[i], tType);
        //                }
        //            }
        //            return mi.Invoke(obj, tmpArgs);
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception(string.Format("Invalid Method Name:{0}", methodname));
        //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    if (ex.InnerException != null && ex.InnerException is System.Net.WebException)
        //    //    {
        //    //        System.Net.WebException webEx = ex.InnerException as System.Net.WebException;
        //    //        if (webEx.Status == WebExceptionStatus.ConnectFailure)
        //    //            throw new Exception("Target WebService connection failure", ex);
        //    //        else if (ex.InnerException.InnerException != null && ex.InnerException.InnerException is System.IO.IOException)
        //    //            if (ex.InnerException.InnerException.InnerException != null && ex.InnerException.InnerException.InnerException is System.Net.Sockets.SocketException)
        //    //            {
        //    //                System.Net.Sockets.SocketException socketEx = ex.InnerException.InnerException.InnerException as System.Net.Sockets.SocketException;
        //    //                if (socketEx.ErrorCode == 10054)
        //    //                    ///存在的连接被远程主机强制关闭。
        //    //                    ///通常原因为：远程主机上对等方应用程序突然停止运行，或远程主机重新启动，或远程主机在远程方套接字上使用了“强制”关闭
        //    //                    ///（参见setsockopt(SO_LINGER)）。另外，在一个或多个操作正在进行时，如果连接因“keep-alive”活动检测到一个失败而中断，
        //    //                    ///也可能导致此错误。此时，正在进行的操作以错误码WSAENETRESET失败返回，后续操作将失败返回错误码WSAECONNRESET。
        //    //                    /// 
        //    //                    ///简单来说就是“超时”！
        //    //                    ///
        //    //                    throw new Exception("Target WebService connection was closed", ex);
        //    //            }
        //    //    }
        //    //    if (ex.InnerException != null)
        //    //        throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
        //    //    else
        //    //        throw ex;
        //    //}
        //}
        public static object InvokeWebServiceByDLL(string fileName, string url, string classname, string methodname, int timeout, object[] args)
        {
            InvokeEntity en = new InvokeEntity(fileName, url, classname, methodname, timeout);
            return InvokeWebServiceByDLL(en, methodname, args);
        }
        public static object InvokeWebServiceByDLL(InvokeEntity entity, string methodname, object[] args)
        {
            object obj = entity.iObject;
            MethodInfo mi = entity.Type.GetMethod(methodname);

            if (mi != null)
            {
                if (args == null)
                    return mi.Invoke(obj, null);
                else
                {
                    ParameterInfo[] paramsInfo = mi.GetParameters();
                    object[] tmpArgs = new object[args.Length];

                    for (int i = 0; i < args.Length; i++)
                    {
                        Type tType = paramsInfo[i].ParameterType;
                        //如果它是值类型,或者String   
                        if (tType.Equals(typeof(string)) || (!tType.IsInterface && !tType.IsClass))
                        {
                            //改变参数类型   
                            tmpArgs[i] = Convert.ChangeType(args[i], tType);
                        }
                    }
                    return mi.Invoke(obj, tmpArgs);
                }
            }
            else
            {
                throw new Exception(string.Format("Invalid Method Name:{0}", entity.MethodName));
            }
        }
        public static bool CreateWebServiceDLL(string url, string classname, string fileName)
        {
            try
            {
                InvokeWebService(url, classname, null, null, fileName);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
        }
        private static object InvokeWebService(string url, string classname, string methodname, object[] args, string fileName)
        {

            if ((classname == null) || (classname == ""))
            {
                classname = GetWsClassName(url);
            }

            try
            {
                //获取WSDL
                WebClient wc = new WebClient();
                Stream stream = wc.OpenRead(url + "?WSDL");

                ServiceDescription sd = ServiceDescription.Read(stream);
                ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                sdi.AddServiceDescription(sd, "", "");
                CodeNamespace cn = new CodeNamespace(@namespace);

                //生成客户端代理类代码
                CodeCompileUnit ccu = new CodeCompileUnit();

                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);
                CSharpCodeProvider csc = new CSharpCodeProvider();
                //ICodeCompiler icc = csc.CreateCompiler();

                //设定编译参数
                CompilerParameters cplist = new CompilerParameters();
                cplist.GenerateExecutable = false;

                if (!string.IsNullOrEmpty(fileName))
                {
                    cplist.OutputAssembly = fileName;
                    cplist.GenerateInMemory = false;
                }
                else
                    cplist.GenerateInMemory = true;

                cplist.ReferencedAssemblies.Add("System.dll");
                cplist.ReferencedAssemblies.Add("System.XML.dll");
                cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                cplist.ReferencedAssemblies.Add("System.Data.dll");

                //编译代理类
                //CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
                CompilerResults cr = csc.CompileAssemblyFromDom(cplist, ccu);
                if (true == cr.Errors.HasErrors)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                    {
                        sb.Append(ce.ToString());
                        sb.Append(System.Environment.NewLine);
                    }
                    throw new Exception(sb.ToString());
                }

                if (string.IsNullOrEmpty(fileName))
                {
                    //生成代理实例，并调用方法
                    Assembly assembly = cr.CompiledAssembly;
                    Type t = assembly.GetType(@namespace + "." + classname, true, true);
                    object obj = Activator.CreateInstance(t);
                    MethodInfo mi = t.GetMethod(methodname);

                    if (mi != null)
                    {
                        if (args == null)
                            return mi.Invoke(obj, null);
                        else
                            return mi.Invoke(obj, args);
                    }
                    else
                    {
                        throw new Exception(string.Format("Invalid Method Name:{0}", methodname));
                    }
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
                else
                    throw ex;
            }
        }
        private static string GetWsClassName(string wsUrl)
        {
            string[] parts = wsUrl.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');

            return pps[0];
        }

        #endregion
    }
}
