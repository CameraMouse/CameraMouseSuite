/*                         Camera Mouse Suite
 *  Copyright (C) 2014, Samual Epstein
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;

namespace CameraMouseSuite
{
    public class CMSTrackingSuiteDirectory
    {
        private SortedList<string, CMSTrackingSuite> trackingSuites = new SortedList<string, CMSTrackingSuite>();
        
        public CMSTrackingSuite[] TrackingSuites
        {
            get
            {
                CMSTrackingSuite[] arr = new CMSTrackingSuite[trackingSuites.Count];
                trackingSuites.Values.CopyTo(arr, 0);
                return arr;
            }
        }

        /*
        public string[] TrackingSuiteNames
        {
            get
            {
                string[] names = new string[trackingSuites.Count];
                trackingSuites.Keys.CopyTo(names, 0);
                return names;
            }
        }

        public string[] TrackingSuiteInformalNames
        {
            get
            {
                string[] informalNames = new string[trackingSuites.Count];
                for (int i = 0; i < trackingSuites.Count; i++)
                {
                    informalNames[i] = trackingSuites.Values[i].InformalName;
                }
                return informalNames;
            }
        }

        public string[] TrackSuiteDescriptions
        {
            get
            {
                string[] descriptions = new string[trackingSuites.Count];
                for (int i = 0; i < trackingSuites.Count; i++)
                {
                    descriptions[i] = trackingSuites.Values[i].Description;
                }
                return descriptions;
            }
        }
        */

        public CMSTrackingSuiteIdentifier[] TrackingIdentifiers
        {
            get
            {

                SortedList<CMSTrackingSuiteIdentifier,object> identifiers = new SortedList<CMSTrackingSuiteIdentifier,object> ();

                foreach (CMSTrackingSuite suite in trackingSuites.Values)
                {
                    identifiers[suite.GetIdentifier()] = null;
                }

                CMSTrackingSuiteIdentifier [] ids = new CMSTrackingSuiteIdentifier[identifiers.Count];
                for(int i = 0; i < identifiers.Count; i++)
                {
                    ids[i] = identifiers.Keys[i];
                }
                return ids;
            }
        }

        public CMSTrackingSuite GetTrackingSuite(string name)
        {
            if (name == null)
                return null;
            if (!trackingSuites.ContainsKey(name))
                return null;
            return trackingSuites[name];
        }

        private object saveEachSuiteMutex = new object();
        public void Save(string configDirectory)
        {
            foreach (CMSTrackingSuite ts in trackingSuites.Values)
            {
                try
                {
                    lock (saveEachSuiteMutex)
                    {
                        Type curType = ts.GetType();
                        string filename = configDirectory + "/" + curType.ToString() + CMSConstants.SUITE_CONFIG_SUFFIX;
                        XmlSerializer xmSer = new XmlSerializer(curType);
                        StreamWriter outFile = new StreamWriter(filename);
                        xmSer.Serialize(outFile, ts);
                        outFile.Close();
                    }
                }
                catch (Exception e) 
                {
                }
            }
        }

        private SortedList<string, Assembly> assemblies;

        public void Load(string configDirectory, string libDirectory)
        {
            if (libDirectory == null)
                throw new Exception("SuiteDirectory is null");
            if (configDirectory == null)
                throw new Exception("ConfigDirectory is null");


            SortedList<Type, string> types = new SortedList<Type, string>(new TypeComparer());

            ArrayList trackingSuitesList = new ArrayList();

            Type trackingType = typeof(CMSTrackingSuite);

            DirectoryInfo info = new DirectoryInfo(libDirectory);

            assemblies = new SortedList<string, Assembly>();
            foreach (FileInfo fileInfo in info.GetFiles())
            {
                if (!fileInfo.Extension.Equals(".dll"))
                    continue;
                try
                {
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFile(fileInfo.FullName);
                    AppDomain.CurrentDomain.Load(assembly.GetName());
                    assemblies[assembly.FullName] = assembly;                    
                }
                catch (Exception e)
                {
                }
            }


            {
                DirectoryInfo userLibInfo = new DirectoryInfo(CMSConstants.USER_LIB_DIRECTORY);
                foreach (FileInfo fileInfo in userLibInfo.GetFiles())
                {
                    if (!fileInfo.Extension.Equals(".dll"))
                        continue;
                    try
                    {
                        Assembly a = System.Reflection.Assembly.LoadFile(fileInfo.FullName);
                        AppDomain.CurrentDomain.Load(a.GetName());
                        assemblies[a.FullName] = a;
                        //foreach (AssemblyName an in a.GetReferencedAssemblies())
                        //AppDomain.CurrentDomain.Load(an);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            foreach (Assembly assembly in assemblies.Values)
            {
                try
                {
                    foreach (Type type in assembly.GetTypes())
                    {

                        if (type.IsSubclassOf(trackingType))
                        {
                            bool cont = false;

                            foreach (Attribute curAttr in System.Attribute.GetCustomAttributes(type))
                            {
                                if (curAttr is CMSIgnoreSuiteAtt)
                                {
                                    cont = true;
                                    break;
                                }
                            }
                            if (cont)
                                continue;                            
                            types[type] = assembly.FullName;
                        }
                    }
                }
                catch(Exception e)
                {
                    
                }

            }

            types[typeof(CMSTrackingSuiteStandard)] = null;
            types[typeof(CMSEmptyTrackingSuite)] = null;


            

            foreach (Type type in types.Keys)
            {
                if (!type.IsSubclassOf(typeof(CMSTrackingSuite)))
                    continue;

                string filename = configDirectory + "/" + type.ToString() + CMSConstants.SUITE_CONFIG_SUFFIX;
                bool fileExists = false;
                if (File.Exists(filename))
                {
                    fileExists = true;
                    StreamReader inFile = null;
                    try
                    {
                        inFile = new StreamReader(filename);
                        XmlSerializer xmSer = new XmlSerializer(type);
                        CMSTrackingSuite trackingSuite = xmSer.Deserialize(inFile) as CMSTrackingSuite;
                        if (trackingSuite == null)
                            throw new Exception();
                        AddSuite(trackingSuite);
                        inFile.Close();
                    }
                    catch (Exception e)
                    {
                        if (inFile != null)
                            inFile.Close();
                        fileExists = false;
                        File.Delete(filename);
                    }
                }


                if (!fileExists)
                {
                    try
                    {
                        CMSTrackingSuite trackingSuite = System.Activator.CreateInstance(type) as CMSTrackingSuite;

                        if (trackingSuite == null)
                            continue;
                        XmlSerializer xmSer = new XmlSerializer(type);
                        FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.Delete | FileShare.ReadWrite);
                        StreamWriter outFile = new StreamWriter(fs);
                        xmSer.Serialize(outFile, trackingSuite);
                        AddSuite(trackingSuite);
                        outFile.Close();
                    }
                    catch (Exception e) { }
                }
            }
        }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (assemblies.ContainsKey(args.Name))
                return assemblies[args.Name];
            return null;
        }

        public void LoadSuite(string configDirectory, Type suiteType)
        {
            if (configDirectory == null)
                throw new Exception("ConfigDirectory is null");

            string filename = configDirectory + "/" + suiteType.ToString() + CMSConstants.SUITE_CONFIG_SUFFIX;
            if (File.Exists(filename))
            {
                try
                {
                    StreamReader inFile = new StreamReader(filename);
                    XmlSerializer xmSer = new XmlSerializer(suiteType);
                    CMSTrackingSuite trackingSuite = xmSer.Deserialize(inFile) as CMSTrackingSuite;
                    if (trackingSuite == null)
                        throw new Exception();
                    AddSuite(trackingSuite);
                    inFile.Close();
                }
                catch (Exception e)
                {
                }
            }
        }

        private void AddSuite(CMSTrackingSuite trackingSuite)
        {
            if (trackingSuites == null)
                return;

            if (this.trackingSuites.ContainsKey(trackingSuite.Name))
            {
                CMSTrackingSuite originalSuite = trackingSuites[trackingSuite.Name];
                originalSuite.Update(trackingSuite);
            }
            else
            {
                trackingSuites[trackingSuite.Name] = trackingSuite;
            }
        }

        private object saveSuiteMutex = new object();
        public void SaveSuite(string configDirectory, string suiteName)
        {
            CMSTrackingSuite ts = trackingSuites[suiteName];
            if (ts == null)
                return;
            try
            {
                lock (saveSuiteMutex)
                {
                    Type curType = ts.GetType();
                    string filename = configDirectory + "/" + curType.ToString() + CMSConstants.SUITE_CONFIG_SUFFIX;
                    XmlSerializer xmSer = new XmlSerializer(curType);
                    StreamWriter outFile = new StreamWriter(filename);
                    xmSer.Serialize(outFile, ts);
                    outFile.Close();
                }
            }
            catch (Exception e)
            {
            }

        }

    }

    class TypeComparer : Comparer<Type>
    {
        public override int Compare(Type x, Type y)
        {
            return x.ToString().CompareTo(y.ToString());
        }
    }

}
