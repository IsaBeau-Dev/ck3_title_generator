using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CK3DefinitionsTool
{
    public partial class CK3DefinitionsApp : Form
    {
        Dictionary<string, CK3Title> titles = new Dictionary<string, CK3Title>();
        public CK3DefinitionsApp()
        {
            InitializeComponent();
        }
        private void save_current_title()
        {
            CK3Title SelectedTitle = titles[SelectedEntry.Text];
            SelectedTitle.pretty_name = PrettyNameBox.Text;
            titles.Remove(SelectedTitle.id_name);
            SelectedTitle.id_name = InternalIDName.Text;
            titles.Add(SelectedTitle.id_name, SelectedTitle);
            SelectedTitle.csv_color = CSVColor.BackColor;
            SelectedTitle.title_color = IGTitleColor.BackColor;
            if (SelectedTitle.title_level != TitleLevel.Text)
            {
                ValidDeJureTitles.Items.Clear();
                SelectedTitle.dejure_titles.Clear();
                switch (TitleLevel.Text)
                {
                    case "Barony":
                        break;
                    case "County":
                        foreach (var title in titles)
                        {
                            CK3Title checked_title = title.Value;
                            if (checked_title.title_level == "Barony")
                            {
                                ValidDeJureTitles.Items.Add(checked_title.id_name, SelectedTitle.dejure_titles.Contains(checked_title));
                            }
                        }
                        break;
                    case "Duchy":
                        foreach (var title in titles)
                        {
                            CK3Title checked_title = title.Value;
                            if (checked_title.title_level == "County")
                            {
                                ValidDeJureTitles.Items.Add(checked_title.id_name, SelectedTitle.dejure_titles.Contains(checked_title));
                            }
                        }
                        break;
                    case "Kingdom":
                        foreach (var title in titles)
                        {
                            CK3Title checked_title = title.Value;
                            if (checked_title.title_level == "Duchy")
                            {
                                ValidDeJureTitles.Items.Add(checked_title.id_name, SelectedTitle.dejure_titles.Contains(checked_title));
                            }
                        }
                        break;
                    case "Empire":
                        foreach (var title in titles)
                        {
                            CK3Title checked_title = title.Value;
                            if (checked_title.title_level == "Kingdom")
                            {
                                ValidDeJureTitles.Items.Add(checked_title.id_name, SelectedTitle.dejure_titles.Contains(checked_title));
                            }
                        }
                        break;
                }
            }
            List<CK3Title> dejure_titles = new List<CK3Title>();
            for (int i = 0; i < ValidDeJureTitles.CheckedItems.Count; i++)
            {
                dejure_titles.Add(titles[ValidDeJureTitles.CheckedItems[i].ToString()]);
            }
            if (titles.ContainsKey(CapitalSelect.Text))
            {
                SelectedTitle.capital_county = titles[CapitalSelect.Text];
            }
            SelectedTitle.dejure_titles = dejure_titles;
            SelectedTitle.title_level = TitleLevel.Text;
            SelectedTitle.religion = ReligionBox.Text;
            SelectedTitle.culture = CultureBox.Text;
            SelectedTitle.holding_type = HoldingSelect.Text;

        }
        private void refresh_gui()
        {
            CapitalSelect.Items.Clear();
            foreach (var title in titles)
            {
                CK3Title checked_title = title.Value;
                if (checked_title.title_level == "County")
                {
                    CapitalSelect.Items.Add(checked_title.id_name);
                }
            }
            CK3Title SelectedTitle = titles[SelectedEntry.Text];
            ValidDeJureTitles.Items.Clear();
            switch (SelectedTitle.title_level.ToString())
            {
                case "Barony":
                    ProvinceID.Enabled = true;
                    CSVColor.Enabled = true;
                    CapitalSelect.Enabled = false;
                    ValidDeJureTitles.Enabled = false;
                    break;
                case "County":
                    ProvinceID.Enabled = false;
                    CSVColor.Enabled = false;
                    CapitalSelect.Enabled = false;
                    ValidDeJureTitles.Enabled = true;
                    foreach (var title in titles)
                    {
                        CK3Title checked_title = title.Value;
                        if (checked_title.title_level == "Barony")
                        {
                            ValidDeJureTitles.Items.Add(checked_title.id_name, SelectedTitle.dejure_titles.Contains(checked_title));
                        }
                    }
                    break;
                case "Duchy":
                    ProvinceID.Enabled = false;
                    CSVColor.Enabled = false;
                    CapitalSelect.Enabled = true;
                    ValidDeJureTitles.Enabled = true;
                    foreach (var title in titles)
                    {
                        CK3Title checked_title = title.Value;
                        if (checked_title.title_level == "County")
                        {
                            ValidDeJureTitles.Items.Add(checked_title.id_name, SelectedTitle.dejure_titles.Contains(checked_title));
                        }
                    }
                    break;
                case "Kingdom":
                    ProvinceID.Enabled = false;
                    CSVColor.Enabled = false;
                    CapitalSelect.Enabled = true;
                    ValidDeJureTitles.Enabled = true;
                    foreach (var title in titles)
                    {
                        CK3Title checked_title = title.Value;
                        if (checked_title.title_level == "Duchy")
                        {
                            ValidDeJureTitles.Items.Add(checked_title.id_name, SelectedTitle.dejure_titles.Contains(checked_title));
                        }
                    }
                    break;
                case "Empire":
                    ProvinceID.Enabled = false;
                    CSVColor.Enabled = false;
                    CapitalSelect.Enabled = true;
                    ValidDeJureTitles.Enabled = true;
                    foreach (var title in titles)
                    {
                        CK3Title checked_title = title.Value;
                        if (checked_title.title_level == "Kingdom")
                        {
                            ValidDeJureTitles.Items.Add(checked_title.id_name, SelectedTitle.dejure_titles.Contains(checked_title));
                        }
                    }
                    break;
            }
            PrettyNameBox.Text = SelectedTitle.pretty_name;
            InternalIDName.Text = SelectedTitle.id_name;
            IGTitleColor.BackColor = SelectedTitle.title_color;
            TitleLevel.Text = SelectedTitle.title_level;
            ReligionBox.Text = SelectedTitle.religion;
            CultureBox.Text = SelectedTitle.culture;
            HoldingSelect.Text = SelectedTitle.holding_type;
            if (ProvinceID.Enabled)
            {
                ProvinceID.Value = SelectedTitle.province_id;
            }
            if (CSVColor.Enabled)
            {
                CSVColor.BackColor = SelectedTitle.csv_color;
            }
            if (CapitalSelect.Enabled && SelectedTitle.capital_county != null)
            {
                CapitalSelect.Text = SelectedTitle.capital_county.id_name;
            }
            ProvinceHierarchy.Nodes.Clear();
            ProvinceHierarchy.BeginUpdate();
            foreach (var title in titles)
            {
                CK3Title checked_title = title.Value;
                if (checked_title.title_level == "Empire")
                {
                    TreeNode BaseNode = ProvinceHierarchy.Nodes.Add(checked_title.id_name);
                    if (checked_title.dejure_titles.Count() >= 1)
                    {
                        foreach (CK3Title kingdom in checked_title.dejure_titles)
                        {
                            TreeNode BaseKingdom = BaseNode.Nodes.Add(kingdom.id_name);
                            if (kingdom.dejure_titles.Count() >= 1)
                            {
                                foreach (CK3Title duchy in kingdom.dejure_titles)
                                {
                                    TreeNode BaseDuchy = BaseKingdom.Nodes.Add(duchy.id_name);
                                    if (duchy.dejure_titles.Count() >= 1)
                                    {
                                        foreach (CK3Title county in duchy.dejure_titles)
                                        {
                                            TreeNode BaseCounty = BaseDuchy.Nodes.Add(county.id_name);
                                            if (county.dejure_titles.Count() >= 1)
                                            {
                                                foreach (CK3Title barony in county.dejure_titles)
                                                {
                                                    BaseCounty.Nodes.Add(barony.id_name);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            ProvinceHierarchy.EndUpdate();
        }
        private void create_title()
        {
            List<CK3Title> dejure_titles = new List<CK3Title>();
            for (int i = 0; i < ValidDeJureTitles.CheckedItems.Count; i++)
            {
                dejure_titles.Add(titles[ValidDeJureTitles.CheckedItems[i].ToString()]);
            }
            CK3Title capital_title_stuff = null;
            if (titles.ContainsKey(CapitalSelect.Text))
            {
                capital_title_stuff = titles[CapitalSelect.Text];
            }
            CK3Title new_title = new CK3Title(ProvinceID.Value, InternalIDName.Text, PrettyNameBox.Text, TitleLevel.Text, CSVColor.BackColor, IGTitleColor.BackColor, capital_title_stuff, dejure_titles, ReligionBox.Text, CultureBox.Text, HoldingSelect.Text);
            titles.Add(new_title.id_name, new_title);
            SelectedEntry.Items.Add(InternalIDName.Text);
            SelectedEntry.Text = InternalIDName.Text;
            refresh_gui();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            create_title();
            oldSize = base.Size;

        }

        private Size oldSize;


        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);

            foreach (Control cnt in this.Controls)
                ResizeAll(cnt, base.Size);

            oldSize = base.Size;
        }
        private void ResizeAll(Control control, Size newSize)
        {
            int width = newSize.Width - oldSize.Width;
            control.Left += (control.Left * width) / oldSize.Width;
            control.Width += (control.Width * width) / oldSize.Width;

            int height = newSize.Height - oldSize.Height;
            control.Top += (control.Top * height) / oldSize.Height;
            control.Height += (control.Height * height) / oldSize.Height;
        }



        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void SelectedEntry_SelectedIndexChanged(object sender, EventArgs e)
        {
            refresh_gui();
        }

        private void TitleSaverButton_Click(object sender, EventArgs e)
        {
            if (titles.ContainsKey(InternalIDName.Text))
            {
                save_current_title();
            }
            else
            {
                foreach (var title in titles)
                {
                    if(title.Value.title_level == "Barony" && title.Value.id_name != InternalIDName.Text && title.Value.province_id == ProvinceID.Value)
                    {
                        refresh_gui(); // prevents ID dupes
                        return;
                    }
                }
                create_title();
            }
            refresh_gui();
            if (TitleLevel.Text == "Barony")
            {
                ProvinceID.Value += 1;
            }
        }

        private void TitleLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AutogenID.Checked)
            {
                generate_id_name();
            }
            CK3Title SelectedTitle = titles[SelectedEntry.Text];
            ValidDeJureTitles.Items.Clear();
            switch (TitleLevel.Text)
            {
                case "Barony":
                    ProvinceID.Enabled = true;
                    CSVColor.Enabled = true;
                    CapitalSelect.Enabled = false;
                    ValidDeJureTitles.Enabled = false;
                    HoldingSelect.Enabled = true;
                    ReligionBox.Enabled = false;
                    CultureBox.Enabled = false;
                    break;
                case "County":
                    ProvinceID.Enabled = false;
                    CSVColor.Enabled = false;
                    CapitalSelect.Enabled = false;
                    ValidDeJureTitles.Enabled = true;
                    HoldingSelect.Enabled = false;
                    ReligionBox.Enabled = true;
                    CultureBox.Enabled = true;
                    foreach (var title in titles)
                    {
                        CK3Title checked_title = title.Value;
                        if (checked_title.title_level == "Barony")
                        {
                            ValidDeJureTitles.Items.Add(checked_title.id_name, SelectedTitle.dejure_titles.Contains(checked_title));
                        }
                    }
                    break;
                case "Duchy":
                    ProvinceID.Enabled = false;
                    CSVColor.Enabled = false;
                    CapitalSelect.Enabled = true;
                    ValidDeJureTitles.Enabled = true;
                    HoldingSelect.Enabled = false;
                    ReligionBox.Enabled = false;
                    CultureBox.Enabled = false;
                    foreach (var title in titles)
                    {
                        CK3Title checked_title = title.Value;
                        if (checked_title.title_level == "County")
                        {
                            ValidDeJureTitles.Items.Add(checked_title.id_name, SelectedTitle.dejure_titles.Contains(checked_title));
                        }
                    }
                    break;
                case "Kingdom":
                    ProvinceID.Enabled = false;
                    CSVColor.Enabled = false;
                    CapitalSelect.Enabled = true;
                    ValidDeJureTitles.Enabled = true;
                    HoldingSelect.Enabled = false;
                    ReligionBox.Enabled = false;
                    CultureBox.Enabled = false;
                    foreach (var title in titles)
                    {
                        CK3Title checked_title = title.Value;
                        if (checked_title.title_level == "Duchy")
                        {
                            ValidDeJureTitles.Items.Add(checked_title.id_name, SelectedTitle.dejure_titles.Contains(checked_title));
                        }
                    }
                    break;
                case "Empire":
                    ProvinceID.Enabled = false;
                    CSVColor.Enabled = false;
                    CapitalSelect.Enabled = true;
                    ValidDeJureTitles.Enabled = true;
                    HoldingSelect.Enabled = false;
                    ReligionBox.Enabled = false;
                    CultureBox.Enabled = false;
                    foreach (var title in titles)
                    {
                        CK3Title checked_title = title.Value;
                        if (checked_title.title_level == "Kingdom")
                        {
                            ValidDeJureTitles.Items.Add(checked_title.id_name, SelectedTitle.dejure_titles.Contains(checked_title));
                        }
                    }
                    break;
            }
        }

        private void CSVColor_Click(object sender, EventArgs e)
        {
            if (ColorPicker.ShowDialog() == DialogResult.OK)
            {
                CSVColor.BackColor = ColorPicker.Color;
            }
        }

        private void IGTitleColor_Click(object sender, EventArgs e)
        {
            if (ColorPicker.ShowDialog() == DialogResult.OK)
            {
                IGTitleColor.BackColor = ColorPicker.Color;
            }
        }

        private void LandedTitlesButton_Click(object sender, EventArgs e)
        {
            OutputCode.Text = "";
            foreach (var empires in titles)
            {
                if (empires.Value.title_level == "Empire")
                {
                    OutputCode.Text += empires.Value.print_landed_title();
                }
            }
        }

        private void DefinitionsButton_Click(object sender, EventArgs e)
        {
            OutputCode.Text = "";
            foreach (var barony in titles)
            {

                if (barony.Value.title_level == "Barony")
                {
                    OutputCode.Text += String.Concat(barony.Value.province_id, ";", barony.Value.csv_color.R, ";", barony.Value.csv_color.G, ";", barony.Value.csv_color.B, ";", barony.Value.id_name, ";x;\n");
                }
            }
        }

        private void LocalizationButton_Click(object sender, EventArgs e)
        {
            OutputCode.Text = "l_english:";
            foreach (var title in titles)
            {
                OutputCode.Text += String.Concat("\n ", title.Value.id_name, ":0 \"", title.Value.pretty_name, "\"");
            }
        }

        private void InternalIDName_TextChanged(object sender, EventArgs e)
        {

        }

        private void PrettyNameBox_TextChanged(object sender, EventArgs e)
        {
            if (AutogenID.Checked)
            {
                generate_id_name();
            }
        }

        private void AutogenID_CheckedChanged(object sender, EventArgs e)
        {
            if (AutogenID.Checked)
            {
                InternalIDName.ReadOnly = true;
                generate_id_name();
            }
            else
            {
                InternalIDName.ReadOnly = false;
            }
        }
        private void generate_id_name()
        {
            switch (TitleLevel.Text)
            {
                case "Barony":
                    InternalIDName.Text = "b_";
                    break;
                case "County":
                    InternalIDName.Text = "c_";
                    break;
                case "Duchy":
                    InternalIDName.Text = "d_";
                    break;
                case "Kingdom":
                    InternalIDName.Text = "k_";
                    break;
                case "Empire":
                    InternalIDName.Text = "e_";
                    break;
            }
            InternalIDName.Text += PrettyNameBox.Text.ToLower().Replace(" ", "_").Replace("'s", "").Replace("'", "");
        }

        private void OutputCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void MapBox_MouseUp(object sender, MouseEventArgs e)
        {
            Bitmap b = new Bitmap(MapBox.Image);
            Color color = b.GetPixel(e.X, e.Y);
            if (EyedropperToggle.Checked)
            {
                if (CopyColors.Checked)
                {
                    CSVColor.BackColor = color;
                    IGTitleColor.BackColor = color;
                }
                else
                {
                    IGTitleColor.BackColor = color;
                }
            }
        }
        public class CK3Title : Object
        {
            public decimal province_id;
            public string id_name;
            public string pretty_name;
            public string title_level;
            public Color csv_color;
            public Color title_color;
            public CK3Title capital_county;
            public List<CK3Title> dejure_titles;
            public string religion;
            public string culture;
            public string holding_type;
            public CK3Title(decimal province_id, string id_name, string pretty_name, string title_level, Color csv_color, Color title_color, CK3Title capital_county, List<CK3Title> dejure_titles, string religion, string culture, string holding_type)
            {
                this.province_id = province_id;
                this.id_name = id_name;
                this.pretty_name = pretty_name;
                this.title_level = title_level;
                this.csv_color = csv_color;
                this.title_color = title_color;
                this.capital_county = capital_county;
                this.dejure_titles = dejure_titles;
                this.religion = religion;
                this.culture = culture;
                this.holding_type = holding_type;
            }
            public string print_landed_title(int recursion_level = 0)
            {
                String final_print = "";
                final_print += String.Concat(String.Concat(Enumerable.Repeat("\t", recursion_level)), id_name, " = {");
                if (title_level == "Barony")
                {
                    final_print += String.Concat("\n", String.Concat(Enumerable.Repeat("\t", recursion_level)), "\tprovince = ", province_id);
                    final_print += String.Concat("\n", String.Concat(Enumerable.Repeat("\t", recursion_level)));
                }
                final_print += String.Concat("\n", String.Concat(Enumerable.Repeat("\t", recursion_level)), "\tcolor = { ", title_color.R, " ", title_color.G, " ", title_color.B, " }");
                final_print += String.Concat("\n", String.Concat(Enumerable.Repeat("\t", recursion_level)), "\tcolor2 = { 255 255 255 }");
                if (capital_county != null && title_level != "County")
                {
                    final_print += String.Concat("\n", String.Concat(Enumerable.Repeat("\t", recursion_level)), "\tcapital = ", capital_county.id_name);
                }
                if (dejure_titles.Count() >= 1)
                {
                    foreach (var title in dejure_titles)
                    {
                        final_print += "\n";
                        final_print += title.print_landed_title(recursion_level + 1);
                    }
                }
                final_print += String.Concat("\n", String.Concat(Enumerable.Repeat("\t", recursion_level)), "}");
                return final_print;
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "png files (*.png)|*.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    MapBox.Image = Image.FromFile(dlg.FileName);
                }
            }
        }
        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void HistoryGenerator_Click(object sender, EventArgs e)
        {
            OutputCode.Text = "";
            foreach (var county in titles)
            {
                if (county.Value.title_level == "County")
                {
                    if (county.Value.dejure_titles.Count > 0)
                    {
                        OutputCode.Text += String.Concat("####", county.Value.id_name, "\n");
                        for (int i = 0; i < county.Value.dejure_titles.Count; i++)
                        {
                            if(i == 0)
                            {
                                OutputCode.Text += String.Concat(county.Value.dejure_titles[i].province_id, " = { #", county.Value.dejure_titles[i].id_name, "\n" );
                                OutputCode.Text += String.Concat("\tculture = ", county.Value.culture, "\n");
                                OutputCode.Text += String.Concat("\treligion = ", county.Value.religion, "\n\n");
                                OutputCode.Text += String.Concat("\tholding = ", county.Value.dejure_titles[i].holding_type, "\n}\n");
                            }
                            else
                            {
                                OutputCode.Text += String.Concat(county.Value.dejure_titles[i].province_id, " = { #", county.Value.dejure_titles[i].id_name, "\n");
                                OutputCode.Text += String.Concat("\tholding = ", county.Value.dejure_titles[i].holding_type, "\n}\n");
                            }
                        }
                    }
                }
            }
        }
    }
}
