//
// Copyright (c) 2017 eppz! mobile, Gergely Borbás (SP)
//
// http://www.twitter.com/_eppz
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//


#if UNITY_IOS

using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;


public class BuildPostProcessor
{
    [PostProcessBuildAttribute(1)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
        if (target == BuildTarget.iOS)
        {
            // Read.
            string projectPath = PBXProject.GetPBXProjectPath(path);
            PBXProject project = new PBXProject();
            project.ReadFromString(File.ReadAllText(projectPath));
            var targetGUID = project.GetUnityMainTargetGuid();
            string framework = project.GetUnityFrameworkTargetGuid();

            // 关闭 UnityFramework 中的 bitcode
            project.SetBuildProperty(framework, "ENABLE_BITCODE", "NO");
            project.SetBuildProperty(targetGUID, "ENABLE_BITCODE", "NO");

            project.SetBuildProperty(framework, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");
            project.SetBuildProperty(PBXProject.GetUnityTestTargetName(), "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES",
                "NO");
            project.SetBuildProperty(targetGUID, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");

            // AddFrameworks(project, targetGUID);
            AddFrameworks(project, framework);
            // Write.
            File.WriteAllText(projectPath, project.WriteToString());
        }
    }

    static void AddFrameworks(PBXProject project, string targetGUID)
    {
        // Frameworks
        project.AddFrameworkToProject(targetGUID, "libz.dylib", false);
        project.AddFrameworkToProject(targetGUID, "libsqlite3.tbd", false);
        project.AddFrameworkToProject(targetGUID, "CoreTelephony.framework", false);
    }
}
#endif