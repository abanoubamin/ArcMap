using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.SystemUI;

namespace DesktopWindowsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public IMapDocument mdoc = new MapDocumentClass();
        public IMap map;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse .mxd Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "mxd files (*.mxd)|*.mxd",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;

                mdoc.Open(openFileDialog1.FileName, "");
                for (int i = 0; i < mdoc.MapCount; i++)// loop ageeb el dataframes eli 3ndi w a7otha f combobox
                {
                    comboBox1.Items.Add(mdoc.get_Map(i).Name);
                }
                comboBox1.SelectedItem = comboBox1.Items[0];
                map = mdoc.get_Map(0); //awel map 3ndi
               
                axMapControl1.Map = map;
                axMapControl1.Refresh(); // sa3at mesh bysht3'al mn 3'erha 
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            axToolbarControl1.SetBuddyControl(axMapControl1);

            axToolbarControl1.AddItem("esriControls.ControlsMapZoomInTool", -1, -1, true, 0, esriCommandStyles.esriCommandStyleIconOnly);
            axToolbarControl1.AddItem("esriControls.ControlsMapZoomOutTool", -1, -1, false, 0, esriCommandStyles.esriCommandStyleIconOnly);
            axToolbarControl1.AddItem("esriControls.ControlsMapFullExtentCommand", -1, -1, false, 0, esriCommandStyles.esriCommandStyleIconOnly);
        }

        private void axMapControl1_OnMouseMove(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseMoveEvent e)
        {
            toolStripStatusLabel1.Text = "X=" + e.x.ToString();
            toolStripStatusLabel2.Text = "Y=" + e.y.ToString();
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
            comboBox2.Items.Clear();
            IMapDocument mdoc = new MapDocumentClass();
            mdoc.Open(textBox1.Text, "");

            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                if (mdoc.get_Map(i).Name == comboBox1.SelectedItem.ToString())
                {
                    map = mdoc.get_Map(i);
                    axMapControl1.Map = map;
                    axMapControl1.Refresh();

                    IEnumLayer allLayers = map.get_Layers();
                    ILayer layer = allLayers.Next();//chosen layer
                    while (layer != null)
                    {
                        checkedListBox1.Items.Add(layer.Name);//to choose layer
                        comboBox2.Items.Add(layer.Name);//to remove layer or definition query
                        layer = allLayers.Next();
                    }
                }
                //check true for all layers
                for (int d = 0; d < checkedListBox1.Items.Count; d++)
                {
                    checkedListBox1.SetItemChecked(d, true);
                }
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //map = mdoc.get_Map(comboBox1.SelectedIndex);
            int index = checkedListBox1.SelectedIndex;
            try
            {
                if (checkedListBox1.GetItemCheckState(index) != CheckState.Checked)
                {
                    map.get_Layer(index).Visible = false;
                    axMapControl1.Map = map;
                    axMapControl1.Refresh();
                }
                else
                {
                    map.get_Layer(index).Visible = true;
                    axMapControl1.Map = map;
                    axMapControl1.Refresh();
                }
            }

            catch
            {
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text != "")
            {
                ILayer layer = mdoc.get_Layer(comboBox1.SelectedIndex, comboBox2.SelectedIndex);
                mdoc.get_Map(comboBox1.SelectedIndex).DeleteLayer(layer);
                map = mdoc.get_Map(comboBox1.SelectedIndex);
                axMapControl1.Map = map;
                axMapControl1.Refresh();
                comboBox2.Items.Remove(layer.Name);
                checkedListBox1.Items.Remove(layer.Name);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                MessageBox.Show("Please select a layer from the comboBox firstly!");
                return;
            }
            if (textBox2.Text != "")
            {
                ILayer sheyakhaLayer = axMapControl1.ActiveView.FocusMap.get_Layer(comboBox2.SelectedIndex);
                IFeatureLayer fsheyakhalayer = sheyakhaLayer as FeatureLayer;
                IFeatureLayerDefinition dlayer = fsheyakhalayer as IFeatureLayerDefinition;
                dlayer.DefinitionExpression = textBox2.Text;
                axMapControl1.Refresh();
            }
        
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                MessageBox.Show("Please select a layer from the comboBox firstly!");
                return;
            }
            Form5 form5 = new Form5(this, map, comboBox2.Text, comboBox2.SelectedIndex, axMapControl1);
            form5.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse .gdb | .mdb Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                //Filter = "gdb files (*.gdb)|*.gdb|*.mdb|(*.mdb)",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string strFullPath = dlg.FileName;
                textBox3.Text = dlg.FileName;
                if (strFullPath == "") return;
                if (strFullPath.Contains(".gdb"))
                {
                    string [] str=strFullPath.Split(new string[] { ".gdb" }, StringSplitOptions.None);
                    strFullPath = str[0] + ".gdb";
                    textBox3.Text = strFullPath;
                    IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                    IWorkspace workspace = pWorkspaceFactory.OpenFromFile(strFullPath, 0);
                    IEnumDataset pnumdataset = workspace.get_Datasets(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTAny);
                    pnumdataset.Reset();
                    IDataset pdata = pnumdataset.Next();
                    while (pdata != null)
                    {
                        if (pdata is IFeatureDataset)
                        {
                            IFeatureWorkspace fws = (IFeatureWorkspace)pWorkspaceFactory.OpenFromFile(strFullPath, 0);
                            IFeatureDataset pfeatdataset = fws.OpenFeatureDataset(pdata.Name);
                            IEnumDataset pnumda = pfeatdataset.Subsets;
                            pnumda.Reset();
                            IDataset pdataset1 = pnumda.Next();
                            while (pdataset1 != null)
                            {
                                if (pdataset1 is IFeatureClass)
                                {
                                    IFeatureLayer feetlayer = new FeatureLayerClass();
                                    feetlayer.FeatureClass = fws.OpenFeatureClass(pdataset1.Name);
                                    //feetlayer.Name = feetlayer.FeatureClass.AliasName;
                                    comboBox3.Items.Add(pdataset1.Name);
                                }
                                pdataset1 = pnumda.Next();
                            }
                        }
                        pdata = pnumdataset.Next();
                    }
                }
                if (strFullPath.Contains(".mdb"))
                {
                    IWorkspaceFactory pWorkspaceFactory = new AccessWorkspaceFactoryClass();
                    IWorkspace workspace = pWorkspaceFactory.OpenFromFile(dlg.FileName, 0);
                    IEnumDataset pnumdataset = workspace.get_Datasets(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTAny);
                    pnumdataset.Reset();
                    IDataset pdata = pnumdataset.Next();
                    IFeatureWorkspace fws = (IFeatureWorkspace)pWorkspaceFactory.OpenFromFile(strFullPath, 0);

                    while (pdata != null)
                    {
                        if (pdata is IFeatureDataset)
                        {
                            IFeatureDataset pfeatdataset = fws.OpenFeatureDataset(pdata.Name);
                            IEnumDataset pnumda = pfeatdataset.Subsets;
                            pnumda.Reset();
                            IDataset pdataset1 = pnumda.Next();
                            while (pdataset1 != null)
                            {
                                if (pdataset1 is IFeatureClass)
                                {
                                    IFeatureLayer feetlayer = new FeatureLayerClass();
                                    feetlayer.FeatureClass = fws.OpenFeatureClass(pdataset1.Name);
                                    comboBox3.Items.Add(pdataset1.Name);
                                }
                                pdataset1 = pnumda.Next();
                            }
                        }
                        else
                        {
                            if (pdata is IFeatureClass)
                            {
                                IFeatureLayer PfeatLayer = new FeatureLayerClass();
                                PfeatLayer.FeatureClass = fws.OpenFeatureClass(pdata.Name);
                                comboBox3.Items.Add(pdata.Name);
                            }
                        }
                        pdata = pnumdataset.Next();
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                IWorkspaceFactory wfactory = new AccessWorkspaceFactoryClass();
                IWorkspace ws = wfactory.OpenFromFile(textBox3.Text, 0);  //makan el database 
                IFeatureWorkspace fws = ws as IFeatureWorkspace;
                IFeatureClass featclass = fws.OpenFeatureClass(comboBox3.SelectedItem.ToString());
                IFeatureLayer featLayer = new FeatureLayerClass();
                featLayer.FeatureClass = featclass;
                featLayer.Name = comboBox3.SelectedItem.ToString();
                mdoc.get_Map(comboBox1.SelectedIndex).AddLayer(featLayer);
                IMap map = mdoc.get_Map(comboBox1.SelectedIndex);
                axMapControl1.Map = map;
                axMapControl1.Refresh();
           
                checkedListBox1.Items.Clear();
                comboBox2.Items.Clear();
                IEnumLayer allLayers = map.get_Layers();
                ILayer layer = allLayers.Next();
                while (layer != null)
                {
                    checkedListBox1.Items.Add(layer.Name);
                    comboBox2.Items.Add(layer.Name);
                    layer = allLayers.Next();
                }
                for (int d = 0; d < checkedListBox1.Items.Count; d++)
                {
                    checkedListBox1.SetItemChecked(d, true);
                }
            }
            catch
            {

            }         
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please browse a .mdx file firstly!");
            }
            else
            {
                Form2 form2 = new Form2(this, map, axMapControl1);
                form2.ShowDialog();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please browse a .mdx file firstly!");
            }
            else
            {
                Form3 form3 = new Form3(this, map, axMapControl1);
                form3.ShowDialog();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please browse a .mdx file firstly!");
            }
            else
            {
                Form4 form4 = new Form4(this, map, axMapControl1);
                form4.ShowDialog();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please browse a .mdx file firstly!");
            }
            else
            {
                map.ClearSelection();
                axMapControl1.Refresh();
            }
        }
    }
}