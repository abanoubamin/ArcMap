﻿using System;
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
    public partial class Form2 : Form
    {
        Form f1;
        IMap map; 
        ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        public Form2(Form1 f1, IMap map, ESRI.ArcGIS.Controls.AxMapControl axMapControl1)
        {

            InitializeComponent();

            this.f1 = f1;
            this.map = map;
            this.axMapControl1 = axMapControl1;

            IEnumLayer allLayers = map.get_Layers();
            ILayer layer = allLayers.Next();
            while (layer != null)
            {
                comboBox1.Items.Add(layer.Name.ToString());
                layer = allLayers.Next();
            }
            comboBox1.SelectedItem = comboBox1.Items[0];

            //add methods to comboBox2
            comboBox2.Items.Add("Create a new selection");
            comboBox2.Items.Add("Add to current selection");
            comboBox2.Items.Add("Remove from current selection");
            comboBox2.SelectedItem = comboBox2.Items[0];

        }

        private void Form2_Load_1(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string selectedlayer = comboBox1.SelectedItem.ToString();
            IEnumLayer allLayers = map.get_Layers();
            ILayer layer = allLayers.Next();
            var fields = layer as ILayerFields;
            while (layer != null)
            {
                if(layer.Name == selectedlayer)
                {
                    fields = layer as ILayerFields;
                    break;
                }
                layer = allLayers.Next();
            }
            for (int i = 0; i < fields.FieldCount; i++)
            {
                listBox1.Items.Add(fields.Field[i].Name);
            }
            if (listBox1.Items.Count > 0)
            {
                listBox1.SelectedItem = listBox1.Items[0];
                button17.Enabled = true;
            }

        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                textBox1.Text += listBox1.SelectedItem.ToString();
                textBox1.Focus();
                textBox1.SelectionStart = textBox1.Text.Length;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button17.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text += " = ";
            textBox1.Focus();
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text += " <> ";
            textBox1.Focus();
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text += " LIKE ";
            textBox1.Focus();
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text += " > ";
            textBox1.Focus();
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text += " >= ";
            textBox1.Focus();
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text += " AND ";
            textBox1.Focus();
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.Text += " < ";
            textBox1.Focus();
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox1.Text += " <= ";
            textBox1.Focus();
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox1.Text += " OR ";
            textBox1.Focus();
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox1.Text += "?";
            textBox1.Focus();
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox1.Text += "*";
            textBox1.Focus();
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            textBox1.Text += "()";
            textBox1.Focus();
            textBox1.SelectionStart = textBox1.Text.Length - 1;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            textBox1.Text += " NOT ";
            textBox1.Focus();
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            textBox1.Text += " IS ";
            textBox1.Focus();
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a field firstly!");
            }
            else
            {
                listBox2.Items.Clear();

                ILayer layer = axMapControl1.ActiveView.FocusMap.get_Layer(comboBox1.SelectedIndex);
                var fields = layer as ILayerFields;

                IFeatureLayer pFeatureLayer = (IFeatureLayer)layer;
                IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
                ITable pTable = (ITable)pFeatureClass;
                ICursor pCursor = pTable.Search(null, false);
                IRow pRow = pCursor.NextRow();
                int fldIndex = fields.FindField(listBox1.SelectedItem.ToString());

                List<string> uniqueVals = new List<string>();
                while (pRow != null)
                {
                    uniqueVals.Add(pRow.get_Value(fldIndex).ToString());
                    pRow = pCursor.NextRow();
                }
                uniqueVals = uniqueVals.Distinct().ToList();
                string[] sortedUniqueVals = uniqueVals.ToArray();
                System.Array.Sort(sortedUniqueVals);
                int[] intSrtdUniqueVals = new int[sortedUniqueVals.Count()];
                for (int i = 0; i < sortedUniqueVals.Count(); i++)
                {
                    int n;
                    bool isNumeric = int.TryParse(sortedUniqueVals[i], out n);
                    if (isNumeric)
                    {
                        intSrtdUniqueVals[i] = int.Parse(sortedUniqueVals[i]);
                    }
                }
                System.Array.Sort(intSrtdUniqueVals);
                for (int i = 0; i < sortedUniqueVals.Count(); i++)
                {
                    int n;
                    bool isNumeric = int.TryParse(sortedUniqueVals[i], out n);
                    if (isNumeric)
                    {
                        listBox2.Items.Add(intSrtdUniqueVals[i]);
                    }
                    else if (sortedUniqueVals[i] == "")
                    {
                        listBox2.Items.Add("NULL");
                    }
                    else
                    {
                        listBox2.Items.Add("'" + sortedUniqueVals[i] + "'");
                    }
                }
                textBox2.Enabled = true;
            }
            button17.Enabled = false;
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            if (listBox2.Text != "")
            {
                textBox1.Text += listBox2.SelectedItem.ToString();
                textBox1.Focus();
                textBox1.SelectionStart = textBox1.Text.Length;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                if (listBox2.Items[i].ToString().ToLower().Contains(textBox2.Text.ToString().ToLower()))
                {
                    listBox2.SelectedIndex = i;
                    break;
                }
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text != "" && textBox1.Text != "")
                {
                    ILayer Layer = axMapControl1.ActiveView.FocusMap.get_Layer(comboBox1.SelectedIndex);
                    IFeatureLayer selectedLayer = Layer as IFeatureLayer;
                    IQueryFilter pFilter = new QueryFilterClass();
                    pFilter.WhereClause = textBox1.Text;
                    IFeatureSelection featSelect = selectedLayer as IFeatureSelection;
                    if (comboBox2.SelectedIndex == 0)
                    {
                        featSelect.SelectFeatures(pFilter, esriSelectionResultEnum.esriSelectionResultNew, false);
                    }
                    if (comboBox2.SelectedIndex == 1)
                    {
                        featSelect.SelectFeatures(pFilter, esriSelectionResultEnum.esriSelectionResultAdd, false);
                    }
                    if (comboBox2.SelectedIndex == 2)
                    {
                        featSelect.SelectFeatures(pFilter, esriSelectionResultEnum.esriSelectionResultSubtract, false);
                    }
                    MessageBox.Show("The expression was successfully verified.");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("There was an error with the expression. \nGeneral function failure [" + comboBox1.Text.ToString() + "] \nSyntax error (missing operator) in query expression '" + textBox1.Text.ToString() + "'.");
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text != "" && textBox1.Text != "")
                {
                    ILayer Layer = axMapControl1.ActiveView.FocusMap.get_Layer(comboBox1.SelectedIndex);
                    IFeatureLayer selectedLayer = Layer as IFeatureLayer;
                    IQueryFilter pFilter = new QueryFilterClass();
                    pFilter.WhereClause = textBox1.Text;
                    IFeatureSelection featSelect = selectedLayer as IFeatureSelection;
                    if (comboBox2.SelectedIndex == 0)
                    {
                        featSelect.SelectFeatures(pFilter, esriSelectionResultEnum.esriSelectionResultNew, false);
                    }
                    if (comboBox2.SelectedIndex == 1)
                    {
                        featSelect.SelectFeatures(pFilter, esriSelectionResultEnum.esriSelectionResultAdd, false);
                    }
                    if (comboBox2.SelectedIndex == 2)
                    {
                        featSelect.SelectFeatures(pFilter, esriSelectionResultEnum.esriSelectionResultSubtract, false);
                    }
                    axMapControl1.Refresh();
                }

                this.Close();
                f1.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("There was an error with the expression. \nGeneral function failure [" + comboBox1.Text.ToString() + "] \nSyntax error (missing operator) in query expression '" + textBox1.Text.ToString() + "'.");
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text != "" && textBox1.Text != "")
                {
                    ILayer Layer = axMapControl1.ActiveView.FocusMap.get_Layer(comboBox1.SelectedIndex);
                    IFeatureLayer selectedLayer = Layer as IFeatureLayer;
                    IQueryFilter pFilter = new QueryFilterClass();
                    pFilter.WhereClause = textBox1.Text;
                    IFeatureSelection featSelect = selectedLayer as IFeatureSelection;
                    if (comboBox2.SelectedIndex == 0)
                    {
                        featSelect.SelectFeatures(pFilter, esriSelectionResultEnum.esriSelectionResultNew, false);
                    }
                    if (comboBox2.SelectedIndex == 1)
                    {
                        featSelect.SelectFeatures(pFilter, esriSelectionResultEnum.esriSelectionResultAdd, false);
                    }
                    if (comboBox2.SelectedIndex == 2)
                    {
                        featSelect.SelectFeatures(pFilter, esriSelectionResultEnum.esriSelectionResultSubtract, false);
                    }
                    axMapControl1.Refresh();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("There was an error with the expression. \nGeneral function failure [" + comboBox1.Text.ToString() + "] \nSyntax error (missing operator) in query expression '" + textBox1.Text.ToString() + "'.");
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
