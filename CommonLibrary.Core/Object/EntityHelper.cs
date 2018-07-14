using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace hwj.CommonLibrary.Object
{
    public class EntityHelper
    {
        private static bool AddNamespaceList(string Namespace, ref  List<string> namespaceList)
        {
            if (string.IsNullOrEmpty(namespaceList.Find(delegate(string s) { return s == Namespace; })))
            {
                namespaceList.Add(Namespace);
                return true;
            }
            else
            {
                return false;
            }
        }
        private static string GenerateCode(object obj, bool isTrim, string propertyInfoName, ref List<string> namespaceList, bool isGroupNamespace)
        {
            int segNum = 0;

            // valueName = string.Empty;
            hwj.CommonLibrary.Object.StringHelper.SpaceString sb = new hwj.CommonLibrary.Object.StringHelper.SpaceString();
            // StringBuilder sb = new StringBuilder();
            // sb.AppendLine();
            Type ct = obj.GetType();
            string valueName = string.Empty;
            if (string.IsNullOrEmpty(propertyInfoName))
            {
                valueName = string.Format("_{0}", ct.Name);
            }
            else
            {
                valueName = propertyInfoName;
            }
            if (isGroupNamespace)
            {
                AddNamespaceList(ct.Namespace, ref namespaceList);
                sb.AppendLine(3, string.Format("{0} {1} = new {2}();", ct.Name, valueName, ct.Name));
            }
            else
            {
                sb.AppendLine(3, string.Format("{0} {1} = new {2}();", ct.FullName.Replace('+', '.'), valueName, ct.FullName.Replace('+', '.')));
            }

            object[] pis = obj.GetType().GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                if (pi.Name == "Capacity" || pi.Name == "Count")
                    continue;

                object v = pi.GetValue(obj, null);

                if (v != null)
                {
                    Type t = v.GetType();
                    if (t.FullName.StartsWith("System") && !t.IsGenericType)
                    {
                        switch (t.FullName)
                        {
                            case "System.String":
                                sb.AppendLine(3, string.Format("{0}.{1} = @\"{2}\";", valueName, pi.Name, isTrim ? v.ToString().Trim() : v.ToString()));//.Replace("\r\n", "\\r\\n")));
                                break;
                            case "System.Char":
                                sb.AppendLine(3, string.Format("{0}.{1} = '{2}';", valueName, pi.Name, v.ToString()));
                                break;
                            case "System.Int32":
                            case "System.Double":
                                sb.AppendLine(3, string.Format("{0}.{1} = {2};", valueName, pi.Name, v.ToString()));
                                break;
                            case "System.Decimal":
                                sb.AppendLine(3, string.Format("{0}.{1} = {2}M;", valueName, pi.Name, v.ToString()));
                                break;
                            case "System.DateTime":
                                DateTime dt = DateTime.MinValue;
                                DateTime.TryParse(v.ToString(), out dt);
                                if (dt == DateTime.MinValue)
                                {
                                    sb.AppendLine(3, string.Format("{0}.{1} = DateTime.MinValue;", valueName, pi.Name));
                                }
                                else if (dt == DateTime.MaxValue)
                                {
                                    sb.AppendLine(3, string.Format("{0}.{1} = DateTime.MaxValue;", valueName, pi.Name));
                                }
                                else
                                {
                                    sb.AppendLine(3, string.Format("{0}.{1} = {2}.Parse(\"{3}\");", valueName, pi.Name, t.FullName.Replace('+', '.'), v.ToString()));
                                }
                                break;
                            case "System.Byte[]":
                                System.Byte[] b = (System.Byte[])v;
                                string s = Encoding.Default.GetString(b);
                                sb.AppendLine(3, string.Format("{0}.{1} = Encoding.Default.GetBytes(\"{2}\");", valueName, pi.Name, s));
                                break;
                            default:
                                sb.AppendLine(3, string.Format("{0}.{1} = {2}.Parse(\"{3}\");", valueName, pi.Name, t.FullName.Replace('+', '.'), v.ToString()));
                                break;
                        }
                    }
                    else if (t.IsEnum)
                    {
                        sb.AppendLine(3, string.Format("{0}.{1} = {2}.{3};", valueName, pi.Name, t.FullName.Replace('+', '.'), v.ToString()));
                    }
                    else if (t.FullName.StartsWith("System.Collections.Generic.List") || t.BaseType.FullName.StartsWith("System.Collections.Generic.List") || t.BaseType.FullName.StartsWith("hwj.DBUtility.Entity.BaseList"))
                    {
                        sb.AppendLine(3, string.Format("#region {0}", pi.Name));
                        if (t.FullName.StartsWith("System.Collections.Generic.List"))
                        {
                            if (isGroupNamespace)
                            {
                                AddNamespaceList(t.GetGenericArguments()[0].Namespace, ref namespaceList);
                                sb.AppendLine(3, string.Format("{0}.{1} = new List<{2}>();", valueName, pi.Name, t.GetGenericArguments()[0].Name));
                            }
                            else
                            {
                                sb.AppendLine(3, string.Format("{0}.{1} = new List<{2}>();", valueName, pi.Name, t.GetGenericArguments()[0].FullName.Replace("+", ".")));
                            }
                        }
                        else
                        {
                            if (isGroupNamespace)
                            {
                                AddNamespaceList(t.Namespace, ref namespaceList);
                                sb.AppendLine(3, string.Format("{0}.{1} = new {2}();", valueName, pi.Name, t.Name));
                            }
                            else
                            {
                                sb.AppendLine(3, string.Format("{0}.{1} = new {2}();", valueName, pi.Name, t.FullName.Replace("+", ".")));
                            }
                        }
                        int count = Convert.ToInt32(v.GetType().GetProperty("Count").GetValue(v, null));

                        segNum = 0;
                        for (int i = 0; i < count; i++)
                        {

                            object listItem = v.GetType().GetProperty("Item").GetValue(v, new object[] { i });


                            string vname = string.Empty;
                            if (string.IsNullOrEmpty(propertyInfoName))
                            {
                                vname = string.Format("_{0}_{1}", pi.Name, segNum);
                            }
                            else
                            {
                                vname = string.Format("{0}_{1}_{2}", propertyInfoName, pi.Name, segNum);
                            }

                            string ret = GenerateCode(listItem, isTrim, vname, ref namespaceList, isGroupNamespace);
                            sb.AppendLine(3, string.Format("#region {0}", vname));
                            sb.Append(ret);
                            sb.AppendLine(3, string.Format("{0}.{1}.Add({2});", valueName, pi.Name, vname));
                            sb.AppendLine(3, "#endregion");

                            segNum++;

                        }
                        sb.AppendLine(3, "#endregion");
                    }
                    else
                    {
                        string vname = string.Empty;
                        if (string.IsNullOrEmpty(propertyInfoName))
                        {
                            vname = string.Format("_{0}", pi.Name);
                        }
                        else
                        {
                            vname = string.Format("{0}_{1}", propertyInfoName, pi.Name);
                        }

                        string ret = GenerateCode(v, isTrim, vname, ref namespaceList, isGroupNamespace);
                        sb.AppendLine(3, string.Format("#region {0}", vname));
                        sb.Append(ret);
                        sb.AppendLine(3, string.Format("{0}.{1} = {2};", valueName, pi.Name, vname));
                        sb.AppendLine(3, "#endregion");
                    }
                }
                else
                {
                    if (!pi.PropertyType.FullName.StartsWith("System") || pi.PropertyType.IsGenericType)
                    {
                        sb.AppendLine(3, string.Format("#region {0}", pi.Name));
                    }
                    sb.AppendLine(3, string.Format("{0}.{1} = {2};", valueName, pi.Name, "null"));
                    if (!pi.PropertyType.FullName.StartsWith("System") || pi.PropertyType.IsGenericType)
                    {
                        sb.AppendLine(3, "#endregion");
                    }

                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 根据对象生成对对象赋值的代码
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="genNamespace">生成代码的命名空间</param>
        /// <param name="genClassName">生成代码的类名</param>
        /// <param name="genFunName">生成代码的方法名</param>
        /// <returns>生成的代码</returns>
        public static string EntityToString(object obj, string genNamespace, string genClassName, string genFunName, bool isTrim, bool isGroupNamespace)
        {
            return EntityToString(obj, genNamespace, genClassName, genFunName, obj.GetType().ToString(), isTrim, isGroupNamespace);
        }

        /// <summary>
        /// 根据对象生成对对象赋值的代码
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="genNamespace">生成代码的命名空间</param>
        /// <param name="genClassName">生成代码的类名</param>
        /// <param name="genFunName">生成代码的方法名</param>
        /// <param name="genFunReturnType">生成代码的方法的返回类型</param>
        /// <returns>生成的代码</returns>
        public static string EntityToString(object obj, string genNamespace, string genClassName, string genFunName, string genFunReturnType, bool isTrim, bool isGroupNamespace)
        {
            //  string name = string.Empty;
            //  int seg = 0;
            List<string> namespaceList = new List<string>();

            Type ct = obj.GetType();
            //  string valueName = string.Format("_{0}", ct.Name);

            string FunCode = GenerateCode(obj, isTrim, "", ref namespaceList, isGroupNamespace);
            hwj.CommonLibrary.Object.StringHelper.SpaceString sb = new hwj.CommonLibrary.Object.StringHelper.SpaceString();

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Text;");
            foreach (string s in namespaceList)
            {
                sb.AppendLine(string.Format("using {0};", s));
            }
            sb.AppendLine("");
            sb.AppendLine(string.Format("namespace {0}", genNamespace));
            sb.AppendLine("{");
            sb.AppendLine(1, string.Format("public class {0}", genClassName));
            sb.AppendLine(1, "{");
            sb.AppendLine(2, string.Format("public static {0} {1}()", genFunReturnType, genFunName));
            sb.AppendLine(2, "{");
            sb.AppendLine(FunCode);
            sb.AppendLine(3, string.Format("return {0};", string.Format("_{0}", ct.Name)));
            sb.AppendLine(2, "}");
            sb.AppendLine(1, "}");
            sb.AppendLine("}");
            return sb.ToString();
        }

        /// <summary>
        /// 根据对象生成对对象赋值的代码并保存到指定文件
        /// </summary>
        /// <param name="fileName">保存文件的地址</param>
        /// <param name="obj">要转换的对象</param>
        /// <param name="genNamespace">生成代码的命名空间</param>
        /// <param name="genClassName">生成代码的类名</param>
        /// <param name="genFunName">生成代码的方法名</param>
        /// <returns></returns>
        public static bool EntityToCSFile(string fileName, object obj, string genNamespace, string genClassName, string genFunName, bool isTrim, bool isGroupNamespace)
        {
            return EntityToCSFile(fileName, obj, genNamespace, genClassName, genFunName, obj.GetType().ToString(), isTrim, isGroupNamespace);
        }

        /// <summary>
        /// 根据对象生成对对象赋值的代码并保存到指定文件
        /// </summary>
        /// <param name="fileName">保存文件的地址</param>
        /// <param name="obj">要转换的对象</param>
        /// <param name="genNamespace">生成代码的命名空间</param>
        /// <param name="genClassName">生成代码的类名</param>
        /// <param name="genFunName">生成代码的方法名</param>
        /// <param name="genFunReturnType">生成代码的方法的返回类型</param>
        /// <returns></returns>
        public static bool EntityToCSFile(string fileName, object obj, string genNamespace, string genClassName, string genFunName, string genFunReturnType, bool isTrim, bool isGroupNamespace)
        {
            //try
            //{
            string strfile = EntityToString(obj, genNamespace, genClassName, genFunName, genFunReturnType, isTrim, isGroupNamespace);

            hwj.CommonLibrary.Object.FileHelper.CreateFile(fileName, strfile);
            return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }
    }
}
