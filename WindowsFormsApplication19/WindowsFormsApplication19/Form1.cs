using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication19
{
    public partial class Form1 : Form
    {
        // ─── Baza asocijacija (5 setova) ───────────────────────────────────────
        private readonly List<Asocijacija> baza = new List<Asocijacija>
        {
            new Asocijacija
            {
                Kolone = new string[,]
                {
                    { "MAČKA",   "PAS",     "KONJ",   "KRAVA"   },
                    { "ŠARAN",   "PASTRMKA","LOSOS",  "ŠTUKA"   },
                    { "RUŽA",    "LALA",    "LJILJAN","MASLAČAK"},
                    { "ORAO",    "GOLUB",   "SOVA",   "LASTAVICA"}
                },
                KoloneRešenja = new[] { "DOMAĆE ŽIVOTINJE", "RIBE", "CVEĆE", "PTICE" },
                KrajnjeRešenje = "ŽIVA BIĆA"
            },
            new Asocijacija
            {
                Kolone = new string[,]
                {
                    { "PARIZ",    "LONDON",  "RIM",    "BERLIN"  },
                    { "BUDIMPEŠTA","BEČ",     "PRAG",   "VARŠAVA" },
                    { "MADRID",   "LISABON", "ATINA",  "AMSTERDAM"},
                    { "OSLO",     "KOPENHAGEN","HELSINKI","STOKHOLM"}
                },
                KoloneRešenja = new[] { "ZAPADNA EVROPA", "CENTRALNA EVROPA", "JUŽNA EVROPA", "SKANDINAVIJA" },
                KrajnjeRešenje = "EVROPSKI GRADOVI"
            },
            new Asocijacija
            {
                Kolone = new string[,]
                {
                    { "FUDBAL",  "KOŠARKA", "ODBOJKA","RUKOMET" },
                    { "TENIS",   "STONI TENIS","BADMINTON","SKVOŠ"},
                    { "PLIVANJE","VATERPOLO","SKOKOVI","SINHRO"  },
                    { "BOX",     "JU-DŽICU", "KARATE","HRVANJE"  }
                },
                KoloneRešenja = new[] { "EKIPNI SPORTOVI", "REKET SPORTOVI", "VODENI SPORTOVI", "BORILAČKI SPORTOVI" },
                KrajnjeRešenje = "SPORTOVI"
            },
            new Asocijacija
            {
                Kolone = new string[,]
                {
                    { "GITARA",  "VIOLINA", "ČELO",   "KONTRABAS"},
                    { "TRUBA",   "TROMBON", "TUBA",   "FRULA"   },
                    { "BUBNJEVI","MARIMBA", "TIMPANI","XILOFON"  },
                    { "KLAVIR",  "ORGULJE", "HARFA",  "SINTISAJZER"}
                },
                KoloneRešenja = new[] { "GUDAČKI", "DUVAČKI", "UDARAČKI", "KLAVIJATURNI" },
                KrajnjeRešenje = "MUZIČKI INSTRUMENTI"
            },
            new Asocijacija
            {
                Kolone = new string[,]
                {
                    { "SRBIJA",  "HRVATSKA","SLOVENIJA","BOSNA"  },
                    { "ITALIJA", "ŠPANIJA", "PORTUGAL","FRANCUSKA"},
                    { "JAPAN",   "KINA",    "KOREJA",  "VIJETNAM"},
                    { "BRAZIL",  "ARGENTINA","ČILE",   "PERU"     }
                },
                KoloneRešenja = new[] { "EX-YU", "JUŽNA EVROPA", "ISTOČNA AZIJA", "JUŽNA AMERIKA" },
                KrajnjeRešenje = "DRŽAVE SVETA"
            }
        };

        // ─── Stanje igre (Multiplayer) ──────────────────────────────────────────
        private Asocijacija trenutnaAsocijacija;
        private int[,] otkriveno;
        private bool[] kolonaRešena;
        private bool krajnjeRešeno;

        private int poeniP1;
        private int poeniP2;
        private int trenutniIgrač = 1;
        private bool poljeOtvorenoUToku = false;

        private readonly Random rnd = new Random();

        // ─── UI kontrole ────────────────────────────────────────────────────────
        private Button[,] dugmad = new Button[4, 4];
        private TextBox[] unosiKolona = new TextBox[4];
        private Button[] dugmadRešiKolonu = new Button[4];
        private TextBox unosKrajnji;
        private Button dugmeRešiKrajnje;

        private Label labelPoeniP1;
        private Label labelPoeniP2;
        private Label labelNaRedu;
        private Button dugmeDalje;
        private Button dugmeNovaIgra;
        private Label labelNaslov;

        // ─── Boje ────────────────────────────────────────────────────────────────
        private readonly Color bojaSkriveno = Color.FromArgb(52, 73, 94);
        private readonly Color bojaOtkriveno = Color.FromArgb(41, 128, 185);
        private readonly Color bojaRešeno = Color.FromArgb(39, 174, 96);
        private readonly Color bojaKrajnje = Color.FromArgb(142, 68, 173);
        private readonly Color bojaGreška = Color.FromArgb(192, 57, 43);
        private readonly Color bojaFonta = Color.White;
        private readonly Color bojaAktivni = Color.FromArgb(241, 196, 15);

        public Form1()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.Font = new Font("Segoe UI", 9f);

            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;

            IzgradiUI();
            NovaIgra();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        // ─── Gradnja korisničkog interfejsa ───────────────────────────────────────
        private void IzgradiUI()
        {
            int ekranskaŠirina = Screen.PrimaryScreen.Bounds.Width;
            int ekranskaVisina = Screen.PrimaryScreen.Bounds.Height;

            int cellW = 165;
            int cellH = 60;
            int gapX = 10;
            int gapY = 10;

            int ukupnaŠirinaIgre = 4 * cellW + 3 * gapX;
            int marginLeft = (ekranskaŠirina - ukupnaŠirinaIgre) / 2;
            int startY = (ekranskaVisina - 480) / 2;

            // Naslov
            labelNaslov = new Label
            {
                Text = "IGRA ASOCIJACIJE",
                Font = new Font("Segoe UI", 24f, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(marginLeft, startY - 90)
            };
            this.Controls.Add(labelNaslov);

            // Indikator ko je na redu / Pobednik
            labelNaRedu = new Label
            {
                Text = "NA REDU: IGRAČ 1",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = bojaAktivni,
                BackColor = Color.FromArgb(44, 62, 80),
                Padding = new Padding(10, 5, 10, 5),
                AutoSize = true,
                Location = new Point(marginLeft, startY - 40)
            };
            this.Controls.Add(labelNaRedu);

            // Poeni Igrač 1
            labelPoeniP1 = new Label
            {
                Text = "Igrač 1: 0",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                AutoSize = true,
                Location = new Point(marginLeft + ukupnaŠirinaIgre - 240, startY - 80)
            };
            this.Controls.Add(labelPoeniP1);

            // Poeni Igrač 2
            labelPoeniP2 = new Label
            {
                Text = "Igrač 2: 0",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.FromArgb(192, 57, 43),
                AutoSize = true,
                Location = new Point(marginLeft + ukupnaŠirinaIgre - 240, startY - 45)
            };
            this.Controls.Add(labelPoeniP2);

            // Dugmad (4x4 mreža)
            for (int red = 0; red < 4; red++)
            {
                for (int kol = 0; kol < 4; kol++)
                {
                    var btn = new Button
                    {
                        Size = new Size(cellW, cellH),
                        Location = new Point(marginLeft + kol * (cellW + gapX),
                                              startY + red * (cellH + gapY)),
                        BackColor = bojaSkriveno,
                        ForeColor = bojaFonta,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                        Text = "?",
                        Tag = new int[] { red, kol }
                    };
                    btn.FlatAppearance.BorderSize = 0;
                    btn.Click += Dugme_Click;
                    this.Controls.Add(btn);
                    dugmad[red, kol] = btn;
                }
            }

            // Unosi i dugmad za kolone
            int rešiY = startY + 4 * (cellH + gapY) + 15;
            for (int kol = 0; kol < 4; kol++)
            {
                int x = marginLeft + kol * (cellW + gapX);

                var tb = new TextBox
                {
                    Size = new Size(cellW, 28),
                    Location = new Point(x, rešiY),
                    Font = new Font("Segoe UI", 11f),
                    TextAlign = HorizontalAlignment.Center
                };
                this.Controls.Add(tb);
                unosiKolona[kol] = tb;

                int kRef = kol;
                var btn = new Button
                {
                    Size = new Size(cellW, 35),
                    Location = new Point(x, rešiY + 35),
                    Text = "REŠI",
                    BackColor = Color.FromArgb(52, 152, 219),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += (s, e) => RešiKolonu(kRef);
                this.Controls.Add(btn);
                dugmadRešiKolonu[kol] = btn;
            }

            // Krajnje rešenje
            int krajnjeY = rešiY + 100;

            var labKrajnje = new Label
            {
                Text = "KRAJNJE REŠENJE:",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(marginLeft, krajnjeY)
            };
            this.Controls.Add(labKrajnje);

            unosKrajnji = new TextBox
            {
                Size = new Size(380, 30),
                Location = new Point(marginLeft, krajnjeY + 26),
                Font = new Font("Segoe UI", 12f),
                TextAlign = HorizontalAlignment.Center
            };
            this.Controls.Add(unosKrajnji);

            dugmeRešiKrajnje = new Button
            {
                Size = new Size(120, 34),
                Location = new Point(marginLeft + 390, krajnjeY + 24),
                Text = "POTVRDI",
                BackColor = Color.FromArgb(142, 68, 173),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold)
            };
            dugmeRešiKrajnje.FlatAppearance.BorderSize = 0;
            dugmeRešiKrajnje.Click += DugmeRešiKrajnje_Click;
            this.Controls.Add(dugmeRešiKrajnje);

            // Dugme DALJE / SLEDEĆA IGRA
            dugmeDalje = new Button
            {
                Size = new Size(180, 45),
                Location = new Point(marginLeft + ukupnaŠirinaIgre - 330, krajnjeY + 18),
                Text = "DALJE",
                BackColor = Color.FromArgb(230, 126, 34),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold)
            };
            dugmeDalje.FlatAppearance.BorderSize = 0;
            dugmeDalje.Click += DugmeDalje_Click;
            this.Controls.Add(dugmeDalje);

            // Nova igra ručni reset
            dugmeNovaIgra = new Button
            {
                Size = new Size(140, 45),
                Location = new Point(marginLeft + ukupnaŠirinaIgre - 140, krajnjeY + 18),
                Text = "NOVA IGRA",
                BackColor = Color.FromArgb(39, 174, 96),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold)
            };
            dugmeNovaIgra.FlatAppearance.BorderSize = 0;
            dugmeNovaIgra.Click += (s, e) => NovaIgra();
            this.Controls.Add(dugmeNovaIgra);
        }

        private void DugmeDalje_Click(object sender, EventArgs e)
        {
            if (krajnjeRešeno)
            {
                NovaIgra();
            }
            else
            {
                PromeniIgrača();
            }
        }

        // ─── Nova igra / Reset ─────────────────────────────────────────────────────
        private void NovaIgra()
        {
            trenutnaAsocijacija = baza[rnd.Next(baza.Count)];
            otkriveno = new int[4, 4];
            kolonaRešena = new bool[4];
            krajnjeRešeno = false;

            poeniP1 = 0;
            poeniP2 = 0;
            trenutniIgrač = 1;
            poljeOtvorenoUToku = false;

            dugmeDalje.Text = "DALJE";
            dugmeDalje.BackColor = Color.FromArgb(230, 126, 34);

            AžurirajPrikazStanja();
            OmogućiSvaSkrivenaPolja();

            for (int r = 0; r < 4; r++)
            {
                for (int k = 0; k < 4; k++)
                {
                    dugmad[r, k].Text = "?";
                    dugmad[r, k].BackColor = bojaSkriveno;
                    dugmad[r, k].Enabled = true;
                }
            }

            for (int k = 0; k < 4; k++)
            {
                unosiKolona[k].Text = "";
                unosiKolona[k].Enabled = true;
                unosiKolona[k].BackColor = SystemColors.Window;
                dugmadRešiKolonu[k].Enabled = true;
                dugmadRešiKolonu[k].BackColor = Color.FromArgb(52, 152, 219);
            }

            unosKrajnji.Text = "";
            unosKrajnji.Enabled = true;
            unosKrajnji.BackColor = SystemColors.Window;
            dugmeRešiKrajnje.Enabled = true;
            dugmeRešiKrajnje.BackColor = Color.FromArgb(142, 68, 173);
        }

        // ─── Klik na polje asocijacije ─────────────────────────────────────────────
        private void Dugme_Click(object sender, EventArgs e)
        {
            if (poljeOtvorenoUToku || krajnjeRešeno) return;

            var btn = (Button)sender;
            var pos = (int[])btn.Tag;
            int red = pos[0], kol = pos[1];

            if (otkriveno[red, kol] == 1 || kolonaRešena[kol]) return;

            otkriveno[red, kol] = 1;
            btn.Text = trenutnaAsocijacija.Kolone[red, kol];
            btn.BackColor = bojaOtkriveno;

            DodajPoene(1);

            poljeOtvorenoUToku = true;
            OnemogućiSvaSkrivenaPolja();
            AžurirajPrikazStanja();
        }

        private bool DaLiSuSvaPoljaOtvorena()
        {
            for (int r = 0; r < 4; r++)
                for (int k = 0; k < 4; k++)
                    if (otkriveno[r, k] == 0 && !kolonaRešena[k])
                        return false;
            return true;
        }

        private string OcistiTekst(string tekst)
        {
            if (string.IsNullOrEmpty(tekst)) return "";
            string s = tekst.Trim().ToUpper();
            s = s.Replace("Č", "C").Replace("Ć", "C").Replace("Š", "S").Replace("Ž", "Z").Replace("Đ", "DJ");
            return s;
        }

        // ─── Rešavanje kolone ──────────────────────────────────────────────────────
        private void RešiKolonu(int kol)
        {
            if (kolonaRešena[kol] || krajnjeRešeno) return;

            if (!poljeOtvorenoUToku && !DaLiSuSvaPoljaOtvorena())
            {
                MessageBox.Show("Moraš prvo otvoriti jedno polje (?) u svom potezu pre nego što pokušaš da rešiš kolonu!", "Upozorenje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string unos = OcistiTekst(unosiKolona[kol].Text);
            string tačno = OcistiTekst(trenutnaAsocijacija.KoloneRešenja[kol]);

            if (unos == tačno)
            {
                kolonaRešena[kol] = true;

                for (int r = 0; r < 4; r++)
                {
                    dugmad[r, kol].Text = trenutnaAsocijacija.Kolone[r, kol];
                    dugmad[r, kol].BackColor = bojaRešeno;
                }

                unosiKolona[kol].Text = trenutnaAsocijacija.KoloneRešenja[kol].ToUpper();
                unosiKolona[kol].BackColor = Color.FromArgb(200, 255, 200);
                unosiKolona[kol].Enabled = false;
                dugmadRešiKolonu[kol].BackColor = bojaRešeno;
                dugmadRešiKolonu[kol].Enabled = false;

                DodajPoene(10);

                // ─── AUTOMATSKA PROMENA IGRAČA NAKON POGOĐENE KOLONE ───
                PromeniIgrača();
            }
            else
            {
                dugmadRešiKolonu[kol].BackColor = bojaGreška;
                var t = new Timer { Interval = 600 };
                t.Tick += (s, ev) =>
                {
                    dugmadRešiKolonu[kol].BackColor = Color.FromArgb(52, 152, 219);
                    t.Stop();
                    t.Dispose();
                };
                t.Start();

                unosiKolona[kol].Text = "";
                PromeniIgrača();
            }
        }

        // ─── Rešavanje krajnjeg rešenja ───────────────────────────────────────────
        private void DugmeRešiKrajnje_Click(object sender, EventArgs e)
        {
            if (krajnjeRešeno) return;

            if (!poljeOtvorenoUToku && !DaLiSuSvaPoljaOtvorena())
            {
                MessageBox.Show("Moraš prvo otvoriti jedno polje (?) u svom potezu pre nego što pokušaš da rešiš asocijaciju!", "Upozorenje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string unos = OcistiTekst(unosKrajnji.Text);
            string tačno = OcistiTekst(trenutnaAsocijacija.KrajnjeRešenje);

            if (unos == tačno)
            {
                krajnjeRešeno = true;
                unosKrajnji.Text = trenutnaAsocijacija.KrajnjeRešenje.ToUpper();
                unosKrajnji.BackColor = Color.FromArgb(200, 255, 200);
                unosKrajnji.Enabled = false;
                dugmeRešiKrajnje.Enabled = false;

                DodajPoene(25);
                OtkrijCeluTabelu();

                string pobednikPoruka = "";
                if (poeniP1 > poeniP2)
                    pobednikPoruka = "🏆 POBEDNIK: IGRAČ 1 (" + poeniP1 + " : " + poeniP2 + ")";
                else if (poeniP2 > poeniP1)
                    pobednikPoruka = "🏆 POBEDNIK: IGRAČ 2 (" + poeniP2 + " : " + poeniP1 + ")";
                else
                    pobednikPoruka = "🤝 NEREREŠENO (" + poeniP1 + " : " + poeniP2 + ")";

                labelNaRedu.Text = pobednikPoruka;
                labelNaRedu.ForeColor = Color.White;
                labelNaRedu.BackColor = bojaRešeno;

                labelPoeniP1.Text = "Igrač 1: " + poeniP1;
                labelPoeniP2.Text = "Igrač 2: " + poeniP2;

                dugmeDalje.Text = "SLEDEĆA IGRA";
                dugmeDalje.BackColor = bojaRešeno;
            }
            else
            {
                dugmeRešiKrajnje.BackColor = bojaGreška;
                var t = new Timer { Interval = 600 };
                t.Tick += (s, ev) =>
                {
                    dugmeRešiKrajnje.BackColor = Color.FromArgb(142, 68, 173);
                    t.Stop();
                    t.Dispose();
                };
                t.Start();

                unosKrajnji.Text = "";
                PromeniIgrača();
            }
        }

        private void OtkrijCeluTabelu()
        {
            for (int r = 0; r < 4; r++)
            {
                for (int k = 0; k < 4; k++)
                {
                    dugmad[r, k].Text = trenutnaAsocijacija.Kolone[r, k];
                    if (!kolonaRešena[k]) dugmad[r, k].BackColor = bojaOtkriveno;
                    dugmad[r, k].Enabled = false;
                }
                if (!kolonaRešena[r])
                {
                    unosiKolona[r].Text = trenutnaAsocijacija.KoloneRešenja[r].ToUpper();
                    unosiKolona[r].Enabled = false;
                    dugmadRešiKolonu[r].Enabled = false;
                }
            }
        }

        private void OnemogućiSvaSkrivenaPolja()
        {
            for (int r = 0; r < 4; r++)
                for (int k = 0; k < 4; k++)
                    if (otkriveno[r, k] == 0 && !kolonaRešena[k])
                        dugmad[r, k].Enabled = false;
        }

        private void OmogućiSvaSkrivenaPolja()
        {
            for (int r = 0; r < 4; r++)
                for (int k = 0; k < 4; k++)
                    if (otkriveno[r, k] == 0 && !kolonaRešena[k])
                        dugmad[r, k].Enabled = true;
        }

        private void PromeniIgrača()
        {
            if (krajnjeRešeno) return;
            trenutniIgrač = (trenutniIgrač == 1) ? 2 : 1;
            poljeOtvorenoUToku = false;
            OmogućiSvaSkrivenaPolja();
            AžurirajPrikazStanja();
        }

        private void DodajPoene(int vrednost)
        {
            if (trenutniIgrač == 1) poeniP1 += vrednost;
            else poeniP2 += vrednost;
        }

        private void AžurirajPrikazStanja()
        {
            if (krajnjeRešeno) return;
            labelPoeniP1.Text = "Igrač 1: " + poeniP1;
            labelPoeniP2.Text = "Igrač 2: " + poeniP2;
            labelNaRedu.Text = "NA REDU: IGRAČ " + trenutniIgrač;
            labelNaRedu.BackColor = Color.FromArgb(44, 62, 80);
            labelNaRedu.ForeColor = bojaAktivni;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }

    public class Asocijacija
    {
        public string[,] Kolone { get; set; }
        public string[] KoloneRešenja { get; set; }
        public string KrajnjeRešenje { get; set; }
    }
}