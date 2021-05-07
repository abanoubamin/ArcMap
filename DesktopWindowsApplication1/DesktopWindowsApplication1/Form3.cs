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
    public partial class Form3 : Form
    {
        Form f1;
        IMap map;
        ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        public Form3(Form1 f1, IMap map, ESRI.ArcGIS.Controls.AxMapControl axMapControl1)
        {
            InitializeComponent();
            this.f1 = f1;
            this.map = map;
            this.axMapControl1 = axMapControl1;
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            IEnumLayer allLayers = map.get_Layers();
            ILayer layer = allLayers.Next();
            while (layer != null)
            {
                comboBox1.Items.Add(layer.Name.ToString());
                layer = allLayers.Next();
            }
            comboBox1.SelectedItem = comboBox1.Items[0];
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                ILayer Layer = axMapControl1.ActiveView.FocusMap.get_Layer(comboBox1.SelectedIndex);
                IFeatureLayer selectedLayer = Layer as IFeatureLayer;
                IFeatureSelection featSel = selectedLayer as IFeatureSelection;
                IEnumIDs idList = featSel.SelectionSet.IDs;
                int index = idList.Next();
                List<int> indexes = new List<int>();
                IFeatureClass featureClass = selectedLayer.FeatureClass;
                ITable pTable = (ITable)featureClass;
                ICursor pCursor = pTable.Search(null, false);
                IRow pRow = pCursor.NextRow();
                List<int> tableId = new List<int>();
                while (pRow != null)
                {
                    tableId.Add((int)pRow.get_Value(0));
                    pRow = pCursor.NextRow();
                }
                while (index != -1)
                {
                    indexes.Add(index);
                    index = idList.Next();
                }
                pCursor = pTable.Search(null, false);
                pRow = pCursor.NextRow();
                dataGridView1.RowCount = tableId.Count + 1;
                dataGridView1.ColumnCount = pRow.Fields.FieldCount;
                for (int i = 0; i < pRow.Fields.FieldCount; i++)
                {
                    dataGridView1.Rows[0].Cells[i].Value = pRow.Fields.Field[i].Name.ToString();
                }
                for (int i = 1; i < tableId.Count() + 1; i++)
                {
                    bool flag = false;
                    if (indexes.Count() > 0)
                    {
                        for (int m = 0; m < indexes.Count(); m++)
                        {
                            if (indexes[m] == tableId[i - 1])
                            {
                                flag = true;
                                break;
                            }
                        }

                    }
                    for (int j = 0; j < pRow.Fields.FieldCount; j++)
                    {
                        if (flag == true)
                        {
                            dataGridView1.Rows[i].Cells[j].Value = pRow.Value[j].ToString();
                            dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Aqua;
                        }
                        else
                        {
                            dataGridView1.Rows[i].Cells[j].Value = pRow.Value[j].ToString();
                        }
                    }
                    pRow = pCursor.NextRow();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("The table is empty!");
            }
        }
    }
}
