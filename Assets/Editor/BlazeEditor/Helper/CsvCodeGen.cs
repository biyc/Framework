//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/21 | Initialize core skeleton |
*/

using System;
using System.Collections.Generic;
using System.IO;
using Blaze.Utility.Helper;
using Blaze.Manage.Csv;
using Blaze.Utility;
using Blaze.Utility.Extend;
using CsCodeGenerator;
using CsCodeGenerator.Enums;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Blaze.Helper
{
    /// <summary>
    /// Csv自动解析器
    /// </summary>
    public class CsvCodeGen
    {
        #region public

        public static string _genPath = Application.dataPath + "/";
        public static string _Poco = "Poco/";
        public static string _Enum = "Enum/";

        public static void Clean()
        {
            // var path = _genPath + "/Csv/";
            // if (Directory.Exists(path))
            // {
            //     Directory.Delete(path, true);
            //     AssetDatabase.DeleteAsset(path.Substring(path.IndexOf("Assets") + "Assets".Length));
            //
            //     AssetDatabase.Refresh();
            // }
            Tuner.Log(Co._("     -= Clean Csv Code Done =-:green:b;"));
        }

        private static Dictionary<string, string> csvPaths = new Dictionary<string, string>();

        /// <summary>
        /// 生成代码
        /// </summary>
        public static void Gen()
        {
            // 清理全局缓存
            csvPaths.Clear();

            // 指定的文件目录
            var path = PathHelper.GetCurrentPath() + @"/Data/CsvData/";
            // csv文件名：结构描述
            Dictionary<string, List<CsvRowDesc>> csvDescs = new Dictionary<string, List<CsvRowDesc>>();
            Dictionary<string, List<CsvEnumData>> enumDatas = new Dictionary<string, List<CsvEnumData>>();

            Dictionary<string, int> csvCheck = new Dictionary<string, int>();

            foreach (var filePath in GameUtility.GetAllFilesInFolder(path))
            {
                // 目录下的单个文件是CSV文件
                if (filePath.EndsWith(".csv"))
                {
                    // 文件名
                    var fileName = filePath.Replace(path, "");
                    // 目录名
                    var pathNameArry = fileName.Split('$');
                    if (pathNameArry.Length != 2) throw new Exception("Xlsx文件名不规范(2)");
                    var pathName = pathNameArry[0];
                    fileName = pathNameArry[1];

                    // 去掉后缀的文件名
                    var fileNameNoSub = fileName.Replace(".csv", "");

                    csvPaths.Add(fileNameNoSub.Replace("@", ""), pathName.Replace(".", "/") + "/");
                    // Tuner.Log($"[>>] {fileNameNoSub.Replace("@", "")} : {pathName}");

                    if (fileName.StartsWith("@"))
                    {
                        // 生成枚举文件

                        // csv完整描述
                        var enumData = new List<CsvEnumData>();

                        var bodyLine = CsvReader.ToListWithDeleteFirstLines(CsvReader.ReadCsvWithPath(filePath), 3);
                        bodyLine.ForEach(line =>
                        {
                            var lines = line.Split(',');
                            if (!lines[0].Trim().Equals(""))
                            {
                                enumData.Add(new CsvEnumData(lines[0].ToInt(), lines[1]));
                            }
                        });

                        var enumName = fileNameNoSub.Replace("@", "");
                        enumDatas.Add(enumName, enumData);
                    }
                    else
                    {
                        if (fileNameNoSub.Contains("&"))
                        {
                            var csvParts = fileNameNoSub.Split('&');
                            var shortName = csvParts[0];
                            if (!csvCheck.ContainsKey(shortName))
                            {
                                // 生成CSV文件
                                // 读取描述信息并添加到要生成的列表中
                                csvDescs.Add(shortName, CsvReader.GetRowDesc(CsvReader.ReadCsvWithPath(filePath)));
                                csvCheck[shortName] = 1;
                                csvPaths.Add(shortName.Replace("@", ""), pathName.Replace(".", "/") + "/");
                            }
                        }
                        else
                        {
                            // 生成CSV文件
                            // 读取描述信息并添加到要生成的列表中
                            Debug.Log("Genfile: " + filePath);
                            csvDescs.Add(fileNameNoSub, CsvReader.GetRowDesc(CsvReader.ReadCsvWithPath(filePath)));
                        }
                    }
                }
            }

            // 生成 枚举
            if (enumDatas.Count != 0)
            {
                GeneratorEnums(enumDatas);
            }

            // 调用CSV生成代码
            if (csvDescs.Count != 0)
            {
                // 生成CSV行描述
                // GeneratorRowTemplate(csvDescs);
                // 生成CSV表信息
                GeneratorTables(csvDescs);
                // 生成CSV静态调用句柄
                // GeneratorCsvHelper(csvDescs);
            }

            Tuner.Log(Co._("     -= Gen Csv Code Done =-:green:b;"));
        }

        #endregion

        #region ####### gen enum code ########

        private class CsvEnumData
        {
            public CsvEnumData(int sn, string name)
            {
                Sn = sn;
                Name = name;
            }

            // 数据所在的编号
            public int Sn { get; set; }

            // 数据的键
            public string Name { get; set; }
        }

        private static void GeneratorEnums(Dictionary<string, List<CsvEnumData>> enumDatas)
        {
            foreach (KeyValuePair<string, List<CsvEnumData>> pair in enumDatas)
            {
                GeneratorEnum(pair.Key, pair.Value);
            }
        }

        private static void GeneratorEnum(String enumName, List<CsvEnumData> enumDatas)
        {
            string typeEnum = enumName + "Enum";

            FileModel complexNumberFile = new FileModel(typeEnum);
            complexNumberFile.Namespace = @"Blaze.Manage.Csv.Enum";

            // 生成枚举并添加到文件中
            EnumModel csvEnum = new EnumModel(enumName);
            foreach (var data in enumDatas)
            {
                csvEnum.EnumValues.Add(new EnumValue(data.Name, data.Sn));
            }

            complexNumberFile.Enums.Add(csvEnum);

            // Tuner.Log($"[>>] {enumName} - {csvPaths[enumName]}");

            var genPath = _genPath + csvPaths[enumName] + _Enum;
            Directory.CreateDirectory(genPath);

            CsGenerator csGenerator = new CsGenerator();
            csGenerator.Files.Add(complexNumberFile);
            csGenerator.Path = _genPath;
            csGenerator.OutputDirectory = csvPaths[enumName] + _Enum;
            csGenerator.CreateFiles();
        }

        #endregion

        #region ####### gen csv code ########

        private static void GeneratorTables(Dictionary<string, List<CsvRowDesc>> csvDescs)
        {
            foreach (KeyValuePair<string, List<CsvRowDesc>> pair in csvDescs)
            {
                GeneratorTable(pair.Key, pair.Value);
            }
        }

        private static void GeneratorTable(String csvName, List<CsvRowDesc> csvDescs)
        {
            // 多文件型生成
            string typeCsv = csvName + "Csv";
            string typeRow = csvName + "Row";

            var usingDirectives = new List<string>
            {
                "System;",
                "System.Collections.Generic;",
                "System.Numerics;",
                "UnityEngine;",
                "Blaze.Utility;",
                "Blaze.Manage.Csv.Enum;",
            };
            string fileNameSpace = @"Blaze.Manage.Csv.Poco";

            ClassModel typeClass = new ClassModel(typeCsv);
            typeClass.BaseClass = string.Format(@"CsvBaseTable<{0}>", typeRow);

            // 处理代码体部分
            Func<List<CsvRowDesc>, string> codeContent = (desc) =>
            {
                String content = "";
                desc.ForEach(row =>
                {
                    if (CsvTypeHelper.GetTransFunc(row.Type) != "ToNull")
                    {
                        content += string.Format($"        name = \"{row.Key}\";\n") +
                                   string.Format(
                                       "                    row.{0} = CsvTypeHelper.{1}(parts[{2}]);\n            ",
                                       row.Key,
                                       CsvTypeHelper.GetTransFunc(row.Type), row.Index);
                    }
                    else
                    {
                        // 使用外部解析器来创建类型
                        content += string.Format($"        name = \"{row.Key}\";\n") +
                                   string.Format("                    row.{0} = {1}.Create(parts[{2}]);\n            ",
                                       row.Key,
                                       row.Type.Replace(":", ""), row.Index);
                    }
                });
                return content;
            };

            Func<List<CsvRowDesc>, string> unid = (desc) =>
            {
                String content = "";
                desc.ForEach(row =>
                {
                    if (row.Type.Contains("Uni:"))
                    {
                        content += string.Format(@"""""+row.{0}+", row.Key);
                    }
                });

                if (content.Equals(""))
                {
                    return "";
                }
                else
                {
                    // return string.Format(@"_unidContent.Add(""""+{0},row);", content.TrimEnd('+'));
                    return string.Format(@"        InitRowByUnid({0},row);", content.TrimEnd('+'));
                }
            };

            var methods = new List<Method>();
            methods.Add(new Method("void", "Load")
            {
                AccessModifier = AccessModifier.Protected,
                KeyWords = new List<KeyWord> {KeyWord.Override},
                Parameters = new List<Parameter> {new Parameter(@"List<string>", "list")},
                BodyLines = new List<string>
                {
                    @"int count = 0;",
                    @"string name = """";",
                    @"foreach (var item in list){",
                    @"    try",
                    @"    {",
                    @"        var parts = item.Split(',');",
                    $"        var row = new {typeRow}();",
                    codeContent(csvDescs),
                    unid(csvDescs),
                    @"        _content.Add(row);",
                    @"        ProcessRow(row);",
                    @"        count++;",
                    @"    }",
                    @"    catch (Exception e)",
                    @"    {",
                    @"        Tuner.Error($""[>>] Parse Table Error: {e} "");",
                    @"        Tuner.Log($""[>>] Table:[[{GetCsvName()}]] - Row:[[{count}]] - Col:[[{name}]]"");",
                    @"        Tuner.Log(item);",
                    @"    }",
                    @"}",
                }
            });
            methods.Add(new Method("string", "GetCsvName")
            {
                KeyWords = new List<KeyWord> {KeyWord.Override},
                BodyLines = new List<string>
                {
                    string.Format(@"return ""{0}${1}.csv"";", csvPaths[csvName].Replace("/", ".").TrimEnd('.'),
                        csvName)
                }
            });
            methods.Add(new Method("void", "ProcessRow")
            {
                Parameters = new List<Parameter> {new Parameter(typeRow, "row")},
                KeyWords = new List<KeyWord> {KeyWord.Virtual},
            });
            typeClass.Methods = methods;

            var attrs = new List<AttributeModel>();
            var attrModel = new AttributeModel($"CsvTable(\"{csvName}\")");
            attrs.Add(attrModel);
            typeClass.Attributes = attrs;

            FileModel pocoFile = new FileModel(typeCsv);
            pocoFile.LoadUsingDirectives(usingDirectives);
            pocoFile.Namespace = fileNameSpace;
            pocoFile.Classes.Add(typeClass);


            // 生成第二段 poco 结构
            ClassModel csvBean = new ClassModel(csvName + "Row");
            var properties = csvBean.Properties;

            csvDescs.ForEach(rowInfo =>
            {
                var rowType = "";
                try
                {
                    rowType = CsvTypeHelper.GetType(rowInfo.Type);
                }
                catch (Exception e)
                {
                    Tuner.Error("CSV 文件有格式有错误，请检查数据：" + csvName + "  " + rowInfo.Key);
                    rowType = e.Message;
                }

                // 修改生成类型 ，生成注释
                properties.Add(new Property(rowType, rowInfo.Key)
                {
                    Comment = rowInfo.Desc.Replace("^comma", ",")
                });
            });
            pocoFile.Classes.Add(csvBean);


            // // 生成最后一段 老样式(通过CsvManager 与 spring 统一管理加载对象)
            // // public static string ChipsSuit ="ChipsSuit";
            // // public static ChipsSuitCsv GetChipsSuitCsv()
            // // {
            // //     return CsvManager._.GetCsv(ChipsSuit) as ChipsSuitCsv;
            // // }
            // ClassModel constClass = new ClassModel("CsvHelper");
            // constClass.AccessModifier = AccessModifier.Public_Static_Partial;
            // var field = new Field($"static string {csvName}", $"=\"{csvName}\"");
            // field.AccessModifier = AccessModifier.Public;
            // constClass.Fields.Add(field);
            // constClass.Methods.Add(new Method(typeCsv, $"Get{typeCsv}")
            // {
            //         AccessModifier = AccessModifier.Public_Static,
            //         BodyLines = new List<string>
            //         {
            //                 string.Format(@"return CsvManager._.GetCsv({0}) as {1};", csvName, typeCsv)
            //         }
            // });
            // pocoFile.Classes.Add(constClass);

            // 生成最后一段 新样式（直接生成新对象，不统一管理，不依赖 spring ，保证了程序的稳定）
            // private static ChipsSuitCsv ChipsSuitCsv;
            // public static ChipsSuitCsv GetChipsSuitCsv()
            // {
            //     return ChipsSuitCsv ?? (ChipsSuitCsv = new ChipsSuitCsv());
            // }
            ClassModel constClass = new ClassModel("CsvHelper");
            constClass.AccessModifier = AccessModifier.Public_Static_Partial;
            var field = new Field($"static {typeCsv} {typeCsv}", "");
            field.AccessModifier = AccessModifier.Private;
            constClass.Fields.Add(field);
            constClass.Methods.Add(new Method(typeCsv, $"Get{typeCsv}")
            {
                AccessModifier = AccessModifier.Public_Static,
                BodyLines = new List<string>
                {
                    string.Format(@"return {0} ?? ({0} = new {0}());", typeCsv)
                }
            });
            pocoFile.Classes.Add(constClass);

            var genPath = _genPath + csvPaths[csvName] + _Poco;
            Directory.CreateDirectory(genPath);

            CsGenerator csGenerator = new CsGenerator();
            csGenerator.Files.Add(pocoFile);
            csGenerator.Path = _genPath;
            csGenerator.OutputDirectory = csvPaths[csvName] + _Poco;
            csGenerator.CreateFiles();
        }

        #endregion
    }
}