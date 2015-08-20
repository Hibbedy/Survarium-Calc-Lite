using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using sur_calc_v3;

namespace WindowsFormsApplication1
{
    public partial class Survarium_Calc : Form
    {
        public Survarium_Calc()
        {
            InitializeComponent();
            FillAmmoList();

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
            comboBox6.SelectedIndex = 0;

            textBox1.Text = "0";
            textBox2.Text = "0";
            textBox3.Text = "0";
            textBox4.Text = "0";
            textBox5.Text = "0";
            textBox6.Text = "0";
            textBox7.Text = "0";

        }

        int currentrange;

        
        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            currentrange = hScrollBar1.Value;
            CalculateFalloff();
        }

        float damage;
        float penetration;

        float damageDealt;

        float range;
        float effectiveRange;
        float rof;

        int armor;

        List<List<float>> ammoList = new List<List<float>>();

        float[] bodyMods = new float[] { 1f, 3f, 0.8f, 0.6f };

        float[] rangeException = new float[] {0f,  }; 

        float maxdmgReduction = 0.4f;

        float shotgunSlug = 1.5f;
        float svd_sv98 = 1.2f;
        float pkp = 1.1f;

        float currAmmoMod;

        bool SGUse;

        bool firstCalcDone;

        float mindmgDistance = 0.6f;

        float resultparse;

        void FillAmmoList()
        {
            ammoList.Add(new List<float> { 1f, 1f });       //".30-30"              1
            ammoList.Add(new List<float> { 1f, 1f });       //".357m"               2
            ammoList.Add(new List<float> { 1f, 1f });       //".45acp"              3
            ammoList.Add(new List<float> { 1f, 1f });       //".45acp p+"           4
            ammoList.Add(new List<float> { 1f, 1f });       //"4.6 x 30"            5
            ammoList.Add(new List<float> { 1f, 1f });       //"5.7 x 28"            6
            ammoList.Add(new List<float> { 1f, 1f });       //"5.45 x 39 7N6M"      7
            ammoList.Add(new List<float> { 0.9f, 1.3f });   //"5.45 x 39 7N10"      8
            ammoList.Add(new List<float> { 1.1f, 0.8f });   //"5.45 x 39 7N22"      9
            ammoList.Add(new List<float> { 1f, 1f });       //"5.56 x 45"           10
            ammoList.Add(new List<float> { 1f, 1f });       // "5.56 x 45 SS109"    11
            ammoList.Add(new List<float> { 1f, 1f });       //"6.5 x 39"            12
            ammoList.Add(new List<float> { 1f, 1f });       //"7.62 x 25"           13
            ammoList.Add(new List<float> { 1f, 1f });       //"7.62 x 39"           14
            ammoList.Add(new List<float> { 0.85f, 1.1f });  //"7.62 x 39 7N23"      15
            ammoList.Add(new List<float> { 1f, 1f });       //"7.62 x 39 m"         16
            ammoList.Add(new List<float> { 1f, 1f });       //"7.62 x 51"           17
            ammoList.Add(new List<float> { 1f, 1f });       //"7.62 x 51 AP"        18
            ammoList.Add(new List<float> { 1f, 1f });       //"7.62 x 54 7N1"       19
            ammoList.Add(new List<float> { 1f, 1f });       //"7.62 x 54 7N13"      20
            ammoList.Add(new List<float> { 1f, 1f });       //"7.62 x 54r old"      21
            ammoList.Add(new List<float> { 1f, 1f });       //"9 x 18 makarov"      22
            ammoList.Add(new List<float> { 0.9f, 1.3f });   //"9 x 18 PBM"          23
            ammoList.Add(new List<float> { 0.9f, 1.3f });   //"9 x 19p 7N21"        24
            ammoList.Add(new List<float> { 1.1f, 0.8f });   //"9 x 19p 7N31"        25
            ammoList.Add(new List<float> { 1f, 1f });       //"9 x 19p para"        26
            ammoList.Add(new List<float> { 1f, 1f });       //"9 x 21"              27
            ammoList.Add(new List<float> { 1f, 1f });       //"12.7 x 54 PT"        28
            ammoList.Add(new List<float> { 1f, 1.125f });   //"12.7 x 54 VPS"       29
            ammoList.Add(new List<float> { 1f, 1f });       //"12.7 x 55"           30
            ammoList.Add(new List<float> { 1f, 1f });       //"12.7 x 99"           31
            ammoList.Add(new List<float> { 1f, 1f });       //"12mm buckshot"       32
            ammoList.Add(new List<float> { 1f, 1f });       //"12mm buckshot 2"     33
            ammoList.Add(new List<float> { 0.6f, 2f });     //"12mm slug"           34
            ammoList.Add(new List<float> { 1f, 1f });       //"PAB9"                35
            ammoList.Add(new List<float> { 1.1f, 0.8f });   //"SP5"                 36
            ammoList.Add(new List<float> { 0.85f, 1.3f });  //"SP6"                 37

        }

        private void button1_Click(object sender, EventArgs e)
        {
            firstCalcDone = true;
            Calculate();
        }

        void Calculate()
        {
            GetDamage();
            GetPen();
            GetRange();
            GetArmor();
            GetRof();

            if (armor - penetration > 0)
            {
                if ((1 - (armor - penetration) / 100) > maxdmgReduction)
                {
                    damageDealt = (float)bodyMods[comboBox4.SelectedIndex] * damage * (1 - (armor - penetration) / 100);
                }
                else
                {
                    damageDealt = (float)bodyMods[comboBox4.SelectedIndex] * damage * maxdmgReduction;
                }
            }
            else
            {
                damageDealt = (float)bodyMods[comboBox4.SelectedIndex] * damage;
            }
            CalculateFalloff();
        }

        void CalculateFalloff()
        {
            if (SGUse == false)
            {
                if (currentrange < effectiveRange || currentrange == effectiveRange)
                {
                    label11.Text = "Damage: " + damageDealt.ToString() + " @ " + currentrange.ToString() + " m";
                    int temp2 = (int)Math.Ceiling((double)(100 / damageDealt));
                    label15.Text = "Killshots: " + temp2.ToString();
                    if (temp2 > 1)
                    {
                        float temp3 = temp2 * rof;
                        label16.Text = "Killtime: " + temp3.ToString() + " seconds";
                    }
                    else
                    {
                        label16.Text = "Killtime: Instant!";
                    }
                    resultparse = damageDealt;
                }
                else if((currentrange > effectiveRange) && (currentrange < range))
                {
                    float temp = damageDealt * (1-((1 - mindmgDistance) / (range - effectiveRange)) * (currentrange - effectiveRange));
                    label11.Text = "Damage: " + temp.ToString() + " @ " + currentrange.ToString() + " m";
                    int temp2 = (int)Math.Ceiling((double)(100 / temp));
                    label15.Text = "Killshots: " + temp2.ToString();
                    if (temp2 > 1)
                    {
                        float temp3 = temp2 * rof;
                        label16.Text = "Killtime: " + temp3.ToString() + " seconds";
                    }
                    else
                    {
                        label16.Text = "Killtime: Instant!";
                    }
                    resultparse = temp;
                }
                else
                {
                    float temp = damageDealt * mindmgDistance;
                    label11.Text = "Damage: " + temp.ToString() + " @ " + currentrange.ToString() + " m";
                    int temp2 = (int)Math.Ceiling((double)(100 /temp));
                    label15.Text = "Killshots: " + temp2.ToString();
                    if (temp2 > 1)
                    {
                        float temp3 = temp2 * rof;
                        label16.Text = "Killtime: " + temp3.ToString() + " seconds";
                    }
                    else
                    {
                        label16.Text = "Killtime: Instant!";
                    }
                    resultparse = temp;
                }
            }
            else
            {
                var temprange = range * currAmmoMod;
                var tempeffectiveRange = effectiveRange * currAmmoMod;

                if (currentrange < tempeffectiveRange || currentrange == tempeffectiveRange)
                {
                    label11.Text = "Damage: " + damageDealt.ToString() + " @ " + currentrange.ToString() + " m";
                    int temp2 = (int)Math.Ceiling((double)(100 / damageDealt));
                    label15.Text = "Killshots: " + temp2.ToString();
                    if (temp2 > 1)
                    {
                        float temp3 = temp2 * rof;
                        label16.Text = "Killtime: " + temp3.ToString() + " seconds";
                    }
                    else
                    {
                        label16.Text = "Killtime: Instant!";
                    }
                    resultparse = damageDealt;
                }
                else if ((currentrange > tempeffectiveRange) && (currentrange < temprange))
                {
                    float temp = damageDealt * (1 - ((1 - mindmgDistance) / (temprange - tempeffectiveRange)) * (currentrange - tempeffectiveRange));
                    label11.Text = "Damage: " + temp.ToString() + " @ " + currentrange.ToString() + " m";
                    int temp2 = (int)Math.Ceiling((double)(100 / temp));
                    label15.Text = "Killshots: " + temp2.ToString();
                    if (temp2 > 1)
                    {
                        float temp3 = temp2 * rof;
                        label16.Text = "Killtime: " + temp3.ToString() + " seconds";
                    }
                    else
                    {
                        label16.Text = "Killtime: Instant!";
                    }
                    resultparse = temp;
                }
                else
                {
                    float temp = damageDealt * mindmgDistance * currAmmoMod;
                    label11.Text = "Damage: " + temp.ToString() + " @ " + currentrange.ToString() + " m";
                    int temp2 = (int)Math.Ceiling((double)(100 / temp));
                    label15.Text = "Killshots: " + temp2.ToString();
                    if (temp2 > 1)
                    {
                        float temp3 = temp2 * rof;
                        label16.Text = "Killtime: " + temp3.ToString() + " seconds";
                    }
                    else
                    {
                        label16.Text = "Killtime: Instant!";
                    }
                    resultparse = temp;
                }
            }
        }


        void GetDamage()
        {
            int dmg1 = 0;
            Int32.TryParse(textBox1.Text, out dmg1);
            int dmg2 = 0;
            Int32.TryParse(textBox2.Text, out dmg2);
            if (textBox2.TextLength == 1)
            {
                dmg2 = dmg2 * 10;
            }

            float dmgMod = 0f;
            dmgMod = (float)1f + comboBox1.SelectedIndex * 0.01f;

            float temp = (float)(dmg1 + (dmg2 * 0.01f));

            damage = (float)temp * dmgMod + temp * ammoList[comboBox6.SelectedIndex].ElementAt(0) - temp;
        }

        void GetPen()
        {
            int pen1 = 0;
            Int32.TryParse(textBox3.Text, out pen1);
            int pen2 = 0;
            Int32.TryParse(textBox4.Text, out pen2);
            if(textBox4.TextLength == 1)
            {
                pen2 = pen2 * 10;
            }

            float penMod = 0f;
            penMod = (float)1f + comboBox2.SelectedIndex * 0.01f;

            float temp = (float)(pen1 + (pen2 * 0.01f));

            penetration = temp * penMod + temp * ammoList[comboBox6.SelectedIndex].ElementAt(1) - temp;
        }

        void GetRange()
        {
            int temp = 0;
            Int32.TryParse(textBox6.Text, out temp);
            float rangeMod = 0f;
            rangeMod = (float)1f+comboBox3.SelectedIndex * 0.01f;
            range = (float)temp * rangeMod;

            if (comboBox5.SelectedIndex == 0)
            {
                effectiveRange = range / 2f;
            }
            else if(comboBox5.SelectedIndex == 1 || comboBox5.SelectedIndex == 7)
            {
                effectiveRange = range / 2f - 5f;
            }
            else if(comboBox5.SelectedIndex == 2 || comboBox5.SelectedIndex == 8)
            {
                effectiveRange = 10f;
            }
            else if (comboBox5.SelectedIndex == 3)
            {
                effectiveRange = 20f;
            }
            else if (comboBox5.SelectedIndex == 4)
            {
                effectiveRange = 30f;
            }
            else if (comboBox5.SelectedIndex == 5 || comboBox5.SelectedIndex == 6)
            {
                effectiveRange = 35f;
            }
        }

        void GetArmor()
        {
            armor = 0;
            Int32.TryParse(textBox5.Text, out armor);
        }
        void GetRof()
        {
            int temp = 0;
            Int32.TryParse(textBox7.Text, out temp);
            rof = (float)60 / temp;
        }


        //block other values than digits

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch !=8)
            {
                e.Handled = true;
            }
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }




        //Calculate on Change

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (firstCalcDone == true)
            {
                Calculate();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (firstCalcDone == true)
            {
                Calculate();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (firstCalcDone == true)
            {
                Calculate();
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (firstCalcDone == true)
            {
                Calculate();
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (firstCalcDone == true)
            {
                Calculate();
            }
        }

        //exceptions checkboxes

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox6.SelectedIndex == 18)
            {
                SGUse = true;
                currAmmoMod = svd_sv98;
            }
            else if (comboBox6.SelectedIndex == 19)
            {
                SGUse = true;
                currAmmoMod = pkp;
            }
            else if (comboBox6.SelectedIndex == 33)
            {
                SGUse = true;
                currAmmoMod = shotgunSlug;
            }
            else
            {
                SGUse = false;
            }
            if (firstCalcDone == true)
            {
                Calculate();
            }
        }
    }
}
