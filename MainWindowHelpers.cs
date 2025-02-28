using System.Runtime.InteropServices;
using System.Windows;

internal static class MainWindowHelpers
{
  
    /*private static void Button_Click_0All(object sender, RoutedEventArgs e)
    {
        if (double.TryParse(ltdlnd.Text, out double ltdlnd1) &&
            ltdlnd1 > 0 &&
            double.TryParse(zzlj.Text, out double zzlj1) &&
            zzlj1 > 0 &&
            //double.TryParse(csnsmd.Text, out double csnsmd1) &&
            double.TryParse(klmd.Text, out double klmd1) &&
            klmd1 > 0 &&
            double.TryParse(ccl.Text, out double ccl1) &&
            ccl1 > 0 &&
            double.TryParse(ltmd.Text, out double ltmd1) &&
            ltmd1 > 0 &&
            double.TryParse(sd.Text, out double sd1) &&
            sd1 > 0 &&

            double.TryParse(lx.Text, out double lx1) &&
            lx1 > 0 &&
            double.TryParse(ly.Text, out double ly1) &&
            ly1 > 0 &&
            //double.TryParse(csnsmd.Text, out double csnsmd1) &&
            double.TryParse(nx.Text, out double nx1) &&
            nx1 > 0 &&
            double.TryParse(ny.Text, out double ny1) &&
            ny1 > 0 &&
            double.TryParse(dt.Text, out double dt1) &&
            dt1 > 0 &&
            double.TryParse(t.Text, out double t1) &&
            t1 > 0 &&
            double.TryParse(jingdu.Text, out double jingdu1) &&
            jingdu1 > 0 &&
            double.TryParse(weidu.Text, out double weidu1) &&
            weidu1 > 0 &&
            double.TryParse(jiaosudu.Text, out double jiaosudu1) &&
            jiaosudu1 > 0 &&
            double.TryParse(k.Text, out double k1) &&
            k1 > 0 &&

            double.TryParse(kqmd.Text, out double kqmd1) &&
            kqmd1 > 0 &&
            double.TryParse(fyl.Text, out double fyl1) &&
            fyl1 > 0 &&
            //double.TryParse(csnsmd.Text, out double csnsmd1) &&
            double.TryParse(pjfs.Text, out double pjfs1) &&
            pjfs1 > 0 &&
            double.TryParse(jd.Text, out double jd1) &&
            //jd1 > 0 &&
            double.TryParse(bc.Text, out double bc1) &&
            bc1 > 0 &&
            double.TryParse(bxj.Text, out double bxj1)
            //&&  bxj1 > 0 
            //double.TryParse(ltmd.Text, out double ltmd1) &&
            //ltmd1 > 0

            )
        {

            MyData.Instance.Type = type.Text;
            MyData.Instance.Ltdlnd = ltdlnd1;
            MyData.Instance.Zzlj = zzlj1;
            //MyData.Instance.Csnsmd = csnsmd1;
            MyData.Instance.Klmd = klmd1;
            MyData.Instance.Ccl = ccl1;
            MyData.Instance.Ltmd = ltmd1;
            MyData.Instance.Sd = sd1;

            //计算公式
            var setting = (MyData.Instance.Klmd - MyData.Instance.Ltmd) * 9.8 * Math.Pow(MyData.Instance.Zzlj, 2) / 18 / MyData.Instance.Ltdlnd;
            var vc = Math.Sqrt(2 * 9.8 * MyData.Instance.Zzlj * (MyData.Instance.Klmd - MyData.Instance.Ltmd) / MyData.Instance.Ltmd);
            var t = 1 / MyData.Instance.Ccl * Math.Pow(MyData.Instance.Sd, 1.0 / 6);
            MessageBox.Show($"setting:{setting}\nvc:{vc}\nt:{t}");


            MyData.Instance.Type = type.Text;
            MyData.Instance.Lx = lx1;
            MyData.Instance.Ly = ly1;
            //MyData.Instance.Csnsmd = csnsmd1;
            MyData.Instance.Nx = nx1;
            MyData.Instance.Ny = ny1;
            MyData.Instance.Dt = dt1;
            MyData.Instance.T = t1;
            MyData.Instance.Jingdu = jingdu1;
            MyData.Instance.Weidu = weidu1;
            MyData.Instance.Jiaosudu = jiaosudu1;
            MyData.Instance.K = k1;
            var f = MyData.Instance.Jiaosudu * 2.0 * Math.Sin(MyData.Instance.Weidu);
            MessageBox.Show($"f(科氏力):{f}");

            MyData.Instance.Type = type.Text;
            MyData.Instance.Kqmd = kqmd1;
            MyData.Instance.Fyl = fyl1;
            //MyData.Instance.Csnsmd = csnsmd1;
            MyData.Instance.Pjfs = pjfs1;
            MyData.Instance.Jd = jd1;
            MyData.Instance.Bc = bc1;
            MyData.Instance.Bxj = bxj1;
            MyData.Instance.Ltmd = ltmd1;
            if (MyData.Instance.Pjfs > 0 && MyData.Instance.Pjfs < 16.808)
            {
                MyData.Instance.WaveH = -0.082 + 0.076 * MyData.Instance.Pjfs + 0.011 * MyData.Instance.Pjfs * MyData.Instance.Pjfs;
            }
            else if (MyData.Instance.Pjfs > 16.808 && MyData.Instance.Pjfs < 40)
            {
                MyData.Instance.WaveH = 0.588 + 0.217 * MyData.Instance.Pjfs;
            }
            else
            {
                MessageBox.Show("波高相关参数不合理");
            }


            var waveH = MyData.Instance.WaveH;
            var waveE = 0.125 * MyData.Instance.Ltmd * 9.8 * MyData.Instance.WaveH * MyData.Instance.WaveH;
            var waveK = 1 / MyData.Instance.Bc;

            MessageBox.Show($"waveH(波高):{waveH}\nwaveE(不知道是啥):{waveE}\nwaveK(波数):{waveK}");


        }
        else
        {
            MessageBox.Show("参数不合理");
        }
    }*/
}