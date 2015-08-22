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

            selectedLanguage = 0;
            ChangeLanguage();
        }

        int selectedLanguage;


        string s_gun_properties_e = "Gun Properties";
        string s_basegundamage_e = "BaseGunDamage";
        string s_basegunpenetration_e = "BaseGunPenetration";
        string s_dmgmod_e = "Damage Mod";
        string s_penmod_e = "Penetration Mod";
        string s_range_e = "Range";
        string s_rangemod_e = "Range Mod";
        string s_ammoused_e = "Ammo used";
        string s_rateoffire_e = "Rate of Fire";
        string s_armor_properties_e = "Armor Properties";
        string s_bodyregion_e = "BodyRegion";
        string s_chest_e = "Chest";
        string s_head_e = "Head";
        string s_legsarms_e = "Legs/Arms";
        string s_handsfeet_e = "Hands/Feet";
        string s_armor_e = "Armor";
        string s_exceptions_e = "Exceptions";
        string s_pleasecheck_e = "Please check if your weapon has an\nunusual effective range:";
        string s_results_e = "Results";
        string s_damage_e = "Damage: ";
        string s_killshots_e = "Killshots: ";
        string s_killtime_e = "Killtime: ";
        string s_calculate_e = "Calculate";
        string s_seconds_e = " seconds";
        string s_instant_e = "Instant!";
        string s_notunusual_e = "No unusual effective range";
        string s_allsg_e = "All shotguns, except Toz34";

        string s_gun_properties_ru = "свойства пистолет";
        string s_basegundamage_ru = "Базовый урон\nпистолета";
        string s_basegunpenetration_ru = "база проникновения\n пистолет";
        string s_dmgmod_ru = "модификация\nповреждения";
        string s_penmod_ru = "модификация\nпроникновение";
        string s_range_ru = "диапазон";
        string s_rangemod_ru = "модификация\nдиапазон";
        string s_ammoused_ru = "боеприпасы\nиспользуются";
        string s_rateoffire_ru = "скорость\nстрельбы";
        string s_armor_properties_ru = "свойства брони";
        string s_bodyregion_ru = "тело область";
        string s_chest_ru = "грудь";
        string s_head_ru = "глава";
        string s_legsarms_ru = "ноги/оружие";
        string s_handsfeet_ru = "руки/футов";
        string s_armor_ru = "броня";
        string s_exceptions_ru = "исключения";
        string s_pleasecheck_ru = "Пожалуйста, проверьте, если\nваше оружие имеет необычную\nэффективный диапазон:";
        string s_results_ru = "результаты";
        string s_damage_ru = "потрава: ";
        string s_killshots_ru = "Выстрелов на\nодно убийство: ";
        string s_killtime_ru = "Время убивать: ";
        string s_calculate_ru = "вычислять";
        string s_seconds_ru = " секунд";
        string s_instant_ru = "мгновенное!";
        string s_notunusual_ru = "обычно эффективный диапазон";
        string s_allsg_ru = "все дробовики, кроме Toz34";

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
                    if (selectedLanguage == 0)
                    {
                        label11.Text = s_damage_e + damageDealt.ToString() + " @ " + currentrange.ToString() + " m";
                    }
                    else
                    {
                        label11.Text = s_damage_ru + damageDealt.ToString() + " @ " + currentrange.ToString() + " m";
                    }
                    int temp2 = (int)Math.Ceiling((double)(100 / damageDealt));
                    if (selectedLanguage == 0)
                    {
                        label15.Text = s_killshots_e + temp2.ToString();
                    }
                    else
                    {
                        label15.Text = s_killshots_ru + temp2.ToString();
                    }
                    if (temp2 > 1)
                    {
                        float temp3 = temp2 * rof;
                        if (selectedLanguage == 0)
                        {
                            label16.Text = s_killtime_e + temp3.ToString() + s_seconds_e;
                        }
                        else
                        {
                            label16.Text = s_killtime_ru + temp3.ToString() + s_seconds_ru;
                        }
                    }
                    else
                    {
                        if (selectedLanguage == 0)
                        {
                            label16.Text = s_killtime_e + s_instant_e;
                        }
                        else
                        {
                            label16.Text = s_killtime_ru + s_instant_ru;
                        }
                    }
                    resultparse = damageDealt;
                }
                else if((currentrange > effectiveRange) && (currentrange < range))
                {
                    float temp = damageDealt * (1-((1 - mindmgDistance) / (range - effectiveRange)) * (currentrange - effectiveRange));
                    if (selectedLanguage == 0)
                    {
                        label11.Text = s_damage_e + damageDealt.ToString() + " @ " + currentrange.ToString() + " m";
                    }
                    else
                    {
                        label11.Text = s_damage_ru + damageDealt.ToString() + " @ " + currentrange.ToString() + " m";
                    }
                    int temp2 = (int)Math.Ceiling((double)(100 / temp));
                    if (selectedLanguage == 0)
                    {
                        label15.Text = s_killshots_e + temp2.ToString();
                    }
                    else
                    {
                        label15.Text = s_killshots_ru + temp2.ToString();
                    }
                    if (temp2 > 1)
                    {
                        float temp3 = temp2 * rof;
                        if (selectedLanguage == 0)
                        {
                            label16.Text = s_killtime_e + temp3.ToString() + s_seconds_e;
                        }
                        else
                        {
                            label16.Text = s_killtime_ru + temp3.ToString() + s_seconds_ru;
                        }
                    }
                    else
                    {
                        if (selectedLanguage == 0)
                        {
                            label16.Text = s_killtime_e + s_instant_e;
                        }
                        else
                        {
                            label16.Text = s_killtime_ru + s_instant_ru;
                        }
                    }
                    resultparse = temp;
                }
                else
                {
                    float temp = damageDealt * mindmgDistance;
                    if (selectedLanguage == 0)
                    {
                        label11.Text = s_damage_e + damageDealt.ToString() + " @ " + currentrange.ToString() + " m";
                    }
                    else
                    {
                        label11.Text = s_damage_ru + damageDealt.ToString() + " @ " + currentrange.ToString() + " m";
                    }
                    int temp2 = (int)Math.Ceiling((double)(100 /temp));
                    if (selectedLanguage == 0)
                    {
                        label15.Text = s_killshots_e + temp2.ToString();
                    }
                    else
                    {
                        label15.Text = s_killshots_ru + temp2.ToString();
                    }
                    if (temp2 > 1)
                    {
                        float temp3 = temp2 * rof;
                        if (selectedLanguage == 0)
                        {
                            label16.Text = s_killtime_e + temp3.ToString() + s_seconds_e;
                        }
                        else
                        {
                            label16.Text = s_killtime_ru + temp3.ToString() + s_seconds_ru;
                        }
                    }
                    else
                    {
                        if (selectedLanguage == 0)
                        {
                            label16.Text = s_killtime_e + s_instant_e;
                        }
                        else
                        {
                            label16.Text = s_killtime_ru + s_instant_ru;
                        }
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
                    if (selectedLanguage == 0)
                    {
                        label11.Text = s_damage_e + damageDealt.ToString() + " @ " + currentrange.ToString() + " m";
                    }
                    else
                    {
                        label11.Text = s_damage_ru + damageDealt.ToString() + " @ " + currentrange.ToString() + " m";
                    }
                    
                    int temp2 = (int)Math.Ceiling((double)(100 / damageDealt));

                    if (selectedLanguage == 0)
                    {
                        label15.Text = s_killshots_e + temp2.ToString();
                    }
                    else
                    {
                        label15.Text = s_killshots_ru + temp2.ToString();
                    }

                    
                    if (temp2 > 1)
                    {
                        float temp3 = temp2 * rof;

                        if (selectedLanguage == 0)
                        {
                            label16.Text = s_killtime_e + temp3.ToString() + s_seconds_e;
                        }
                        else
                        {
                            label16.Text = s_killtime_ru + temp3.ToString() + s_seconds_ru;
                        }

                        
                    }
                    else
                    {
                        if (selectedLanguage == 0)
                        {
                            label16.Text = s_killtime_e + s_instant_e;
                        }
                        else
                        {
                            label16.Text = s_killtime_ru + s_instant_ru;
                        }
                    }
                    resultparse = damageDealt;
                }
                else if ((currentrange > tempeffectiveRange) && (currentrange < temprange))
                {
                    float temp = damageDealt * (1 - ((1 - mindmgDistance) / (temprange - tempeffectiveRange)) * (currentrange - tempeffectiveRange));

                    if (selectedLanguage == 0)
                    {
                        label11.Text = s_damage_e + damageDealt.ToString() + " @ " + currentrange.ToString() + " m";
                    }
                    else
                    {
                        label11.Text = s_damage_ru + damageDealt.ToString() + " @ " + currentrange.ToString() + " m";
                    }

                    int temp2 = (int)Math.Ceiling((double)(100 / temp));

                    if (selectedLanguage == 0)
                    {
                        label15.Text = s_killshots_e + temp2.ToString();
                    }
                    else
                    {
                        label15.Text = s_killshots_ru + temp2.ToString();
                    }

                    if (temp2 > 1)
                    {
                        float temp3 = temp2 * rof;
                        
                        if (selectedLanguage == 0)
                        {
                            label16.Text = s_killtime_e + temp3.ToString() + s_seconds_e;
                        }
                        else
                        {
                            label16.Text = s_killtime_ru + temp3.ToString() + s_seconds_ru;
                        }
                    }
                    else
                    {
                        if (selectedLanguage == 0)
                        {
                            label16.Text = s_killtime_e + s_instant_e;
                        }
                        else
                        {
                            label16.Text = s_killtime_ru + s_instant_ru;
                        }
                    }
                    resultparse = temp;
                }
                else
                {
                    float temp = damageDealt * mindmgDistance * currAmmoMod;
                    if (selectedLanguage == 0)
                    {
                        label11.Text = s_damage_e + damageDealt.ToString() + " @ " + currentrange.ToString() + " m";
                    }
                    else
                    {
                        label11.Text = s_damage_ru + damageDealt.ToString() + " @ " + currentrange.ToString() + " m";
                    }
                    int temp2 = (int)Math.Ceiling((double)(100 / temp));
                    if (selectedLanguage == 0)
                    {
                        label15.Text = s_killshots_e + temp2.ToString();
                    }
                    else
                    {
                        label15.Text = s_killshots_ru + temp2.ToString();
                    }
                    if (temp2 > 1)
                    {
                        float temp3 = temp2 * rof;
                        if (selectedLanguage == 0)
                        {
                            label16.Text = s_killtime_e + temp3.ToString() + s_seconds_e;
                        }
                        else
                        {
                            label16.Text = s_killtime_ru + temp3.ToString() + s_seconds_ru;
                        }
                    }
                    else
                    {
                        if (selectedLanguage == 0)
                        {
                            label16.Text = s_killtime_e + s_instant_e;
                        }
                        else
                        {
                            label16.Text = s_killtime_ru + s_instant_ru;
                        }
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

        //Localization

        private void label24_Click(object sender, EventArgs e)
        {
            selectedLanguage = 0;
            ChangeLanguage();
        }

        private void label25_Click(object sender, EventArgs e)
        {
            selectedLanguage = 1;
            ChangeLanguage();
        }

        void ChangeLanguage()
        {
            if (selectedLanguage == 0)
            {
                label1.Text = s_basegundamage_e;
                label2.Text = s_basegunpenetration_e;
                label5.Text = s_dmgmod_e;
                label6.Text = s_penmod_e;
                label7.Text = s_range_e;
                label8.Text = s_rangemod_e;
                label9.Text = s_armor_e;
                label10.Text = s_bodyregion_e;
                label11.Text = s_damage_e;
                label15.Text = s_killshots_e;
                label16.Text = s_killtime_e;
                label17.Text = s_rateoffire_e;
                label18.Text = s_pleasecheck_e;
                label19.Text = s_ammoused_e;
                label20.Text = s_gun_properties_e;
                label21.Text = s_armor_properties_e;
                label22.Text = s_exceptions_e;
                label23.Text = s_results_e;
                button1.Text = s_calculate_e;

                comboBox4.Items[0] = s_chest_e;
                comboBox4.Items[1] = s_head_e;
                comboBox4.Items[2] = s_legsarms_e;
                comboBox4.Items[3] = s_handsfeet_e;

                comboBox5.Items[0] = s_notunusual_e;
                comboBox5.Items[1] = s_allsg_e;
            }
            else if (selectedLanguage == 1)
            {
                label1.Text = s_basegundamage_ru;
                label2.Text = s_basegunpenetration_ru;
                label5.Text = s_dmgmod_ru;
                label6.Text = s_penmod_ru;
                label7.Text = s_range_ru;
                label8.Text = s_rangemod_ru;
                label9.Text = s_armor_ru;
                label10.Text = s_bodyregion_ru;
                label11.Text = s_damage_ru;
                label15.Text = s_killshots_ru;
                label16.Text = s_killtime_ru;
                label17.Text = s_rateoffire_ru;
                label18.Text = s_pleasecheck_ru;
                label19.Text = s_ammoused_ru;
                label20.Text = s_gun_properties_ru;
                label21.Text = s_armor_properties_ru;
                label22.Text = s_exceptions_ru;
                label23.Text = s_results_ru;
                button1.Text = s_calculate_ru;

                comboBox4.Items[0] = s_chest_ru;
                comboBox4.Items[1] = s_head_ru;
                comboBox4.Items[2] = s_legsarms_ru;
                comboBox4.Items[3] = s_handsfeet_ru;

                comboBox5.Items[0] = s_notunusual_ru;
                comboBox5.Items[1] = s_allsg_ru;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://eu.survarium.com/forum/eu-en/viewtopic.php?p=148430#p148430");
        }
    }
}
