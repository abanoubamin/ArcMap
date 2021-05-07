using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
    public partial class Form4 : Form
    {
        Form f1;
        IMap map;
        ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        public Form4(Form1 f1, IMap map, ESRI.ArcGIS.Controls.AxMapControl axMapControl1)
        {
            InitializeComponent();

            this.f1 = f1;
            this.map = map;
            this.axMapControl1 = axMapControl1;

            comboBox1.Items.Add("select features from");
            comboBox1.Items.Add("add to the currently selected features in");
            comboBox1.Items.Add("remove from the currently selected features in");
            comboBox1.Items.Add("select from the currently selected features in");
            comboBox1.SelectedItem = comboBox1.Items[0];

            IEnumLayer allLayers = map.get_Layers();
            ILayer layer = allLayers.Next();
            while (layer != null)
            {
                checkedListBox1.Items.Add(layer.Name.ToString());
                layer = allLayers.Next();
            }

            allLayers = map.get_Layers();
            layer = allLayers.Next();
            while (layer != null)
            {
                comboBox2.Items.Add(layer.Name.ToString());
                layer = allLayers.Next();
            }
            comboBox2.SelectedItem = comboBox2.Items[0];

            comboBox3.SelectedItem = comboBox3.Items[0];
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select Target layer(s) firstly!");
                return;
            }
            try
            {
                ILayer Layer = axMapControl1.ActiveView.FocusMap.get_Layer(comboBox2.SelectedIndex);
                IFeatureLayer selectedLayer = Layer as IFeatureLayer;
                if (selectedLayer != null)
                {
                    IFeatureSelection featSel = selectedLayer as IFeatureSelection;
                    IEnumIDs idList = featSel.SelectionSet.IDs;
                    int index = idList.Next();
                    IFeatureClass featureClass = selectedLayer.FeatureClass;
                    while (index != -1)
                    {
                        IFeature feature = featureClass.GetFeature(index);
                        index = idList.Next();
                        IGeometry geometry = feature.Shape as IGeometry;
                        ISpatialFilter spf = new SpatialFilterClass();
                        spf.Geometry = feature.Shape;
                        if (comboBox3.SelectedIndex == 0)
                        {
                            spf.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        }
                        else if (comboBox3.SelectedIndex == 1)
                        {
                            spf.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                        }
                        else if (comboBox3.SelectedIndex == 2)
                        {
                            spf.SpatialRel = esriSpatialRelEnum.esriSpatialRelWithin;
                        }
                        else if (comboBox3.SelectedIndex == 3)
                        {
                            spf.SpatialRel = esriSpatialRelEnum.esriSpatialRelTouches;
                        }
                        else if (comboBox3.SelectedIndex == 4)
                        {
                            spf.SpatialRel = esriSpatialRelEnum.esriSpatialRelCrosses;
                        }
                        IFeatureLayer sheyakhatLayer = axMapControl1.ActiveView.FocusMap.get_Layer(checkedListBox1.SelectedIndex) as IFeatureLayer;
                        IFeatureSelection featSelect = sheyakhatLayer as IFeatureSelection;
                        if (comboBox1.SelectedIndex == 0)
                        {
                            featSelect.SelectFeatures(spf, esriSelectionResultEnum.esriSelectionResultNew, false);
                        }
                        else if (comboBox1.SelectedIndex == 1)
                        {
                            featSelect.SelectFeatures(spf, esriSelectionResultEnum.esriSelectionResultAdd, false);
                        }
                        else if (comboBox1.SelectedIndex == 2)
                        {
                            featSelect.SelectFeatures(spf, esriSelectionResultEnum.esriSelectionResultSubtract, false);
                        }
                        else if (comboBox1.SelectedIndex == 3)
                        {
                            featSelect.SelectFeatures(spf, esriSelectionResultEnum.esriSelectionResultAnd, false);
                        }
                        axMapControl1.ActiveView.Refresh();
                    }
                }
                this.Close();
                f1.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            /*if (map.SelectionCount == 0)
                MessageBox.Show("count = 0");
            else
            {
                //GET ALL SELECTED FEATURES
                IEnumFeature enumFeature = (IEnumFeature)map.FeatureSelection;

                //DEFAULT FEATURE SELECTION ONLY INCLUDES SHAPE, GET ALL FIELDS
                IEnumFeatureSetup enumFeatSetup = (IEnumFeatureSetup)enumFeature;
                enumFeatSetup.AllFields = true;

                //LOOP SELECTED FEATURES AND DO SOMETHING WITH THEM
                enumFeature.Reset();
                IFeature selectedFeature = enumFeature.Next();

                
                while (selectedFeature != null)
                {
                    MessageBox.Show(selectedFeature.Value[0].ToString());
                    selectedFeature = enumFeature.Next();
                }
            }*/

        }
    }
}
