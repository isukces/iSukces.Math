using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xunit;
#if COREFX
using iSukces.Mathematics.Compatibility;
#else
using System;
using System.Windows.Media.Media3D;
#endif

namespace iSukces.Mathematics.test
{
    public class Coordinates3DTests
    {
        [Fact]
        public void T01_Should_invert()
        {
            var m = new Coordinates3D(
                new Vector3D(1, 0, 0),
                new Vector3D(0, 1, 0),
                new Point3D()
            );
            var mInv = m.Reversed;
            Assert.Equal(1, mInv.X.X);
            Assert.Equal(0, mInv.X.Y);
            Assert.Equal(0, mInv.X.Z);
            Assert.Equal(0, mInv.Y.X);
            Assert.Equal(1, mInv.Y.Y);
            Assert.Equal(0, mInv.Y.Z);
            Assert.Equal(0, mInv.Origin.X);
            Assert.Equal(0, mInv.Origin.Y);
            Assert.Equal(0, mInv.Origin.Z);
        }


        [Fact]
        public void T02_Should_invert()
        {
            var m = new Coordinates3D(
                new Vector3D(0, 1, 0),
                new Vector3D(0, 0, 1),
                new Point3D()
            );
            var mInv = m.Reversed;
            Assert.Equal(0, mInv.X.X);
            Assert.Equal(0, mInv.X.Y);
            Assert.Equal(1, mInv.X.Z);
            Assert.Equal(1, mInv.Y.X);
            Assert.Equal(0, mInv.Y.Y);
            Assert.Equal(0, mInv.Y.Z);
            Assert.Equal(0, mInv.Origin.X);
            Assert.Equal(0, mInv.Origin.Y);
            Assert.Equal(0, mInv.Origin.Z);
        }

        [Fact]
        public void T03_Should_invert()
        {
            var m = new Coordinates3D(
                new Vector3D(0, 1, 0),
                new Vector3D(1, 0, 0),
                new Point3D()
            );
            var mInv = m.Reversed;
            Assert.Equal(0, mInv.X.X);
            Assert.Equal(1, mInv.X.Y);
            Assert.Equal(0, mInv.X.Z);
            Assert.Equal(1, mInv.Y.X);
            Assert.Equal(0, mInv.Y.Y);
            Assert.Equal(0, mInv.Y.Z);
            Assert.Equal(0, mInv.Origin.X);
            Assert.Equal(0, mInv.Origin.Y);
            Assert.Equal(0, mInv.Origin.Z);
        }

        [Fact]
        public void T04_Should_invert()
        {
            var m = new Coordinates3D(
                new Vector3D(1.2, -0.9, 0),
                new Vector3D(0.1, 0.134, 3),
                new Point3D(1.234, -2.33, 4.99)
            );

            var mInv = m.Reversed;
            Assert.Equal(0.8, mInv.X.X, 14);
            Assert.Equal(0.0333881849416147, mInv.X.Y, 14);
            Assert.Equal(-0.59907030397634, mInv.X.Z, 14);
            Assert.Equal(-0.6, mInv.Y.X, 14);
            Assert.Equal(0.0445175799221529, mInv.Y.Y, 14);
            Assert.Equal(-0.798760405301787, mInv.Y.Z, 14);
            Assert.Equal(-2.3852, mInv.Origin.X, 14);
            Assert.Equal(-4.91974308706923, mInv.Origin.Y, 14);
            Assert.Equal(-1.39953739401079, mInv.Origin.Z, 14);
        }


        [Fact]
        public void T05_Should_invert()
        {
#if ALLFEATURES
            Coordinates3DGenerator.GetCodeMany();
#endif

            var m = new Coordinates3D(new Vector3D(-0.685233595272487, 0.107679735486363, -0.720319369776719),
                new Vector3D(0.540795206478047, 0.737686066608701, -0.404177945689457),
                new Point3D(1.12404705310429, 3.06736048686661, -0.452741694381806));
            var mInv = m.Reversed;
            Assert.Equal(-0.685233595272487, mInv.X.X, 14);
            Assert.Equal(0.540795206478047, mInv.X.Y, 14);
            Assert.Equal(0.487847788311384, mInv.X.Z, 14);
            Assert.Equal(0.107679735486363, mInv.Y.X, 14);
            Assert.Equal(0.737686066608701, mInv.Y.Y, 14);
            Assert.Equal(-0.666501569163172, mInv.Y.Z, 14);
            Assert.Equal(0.113823625618232, mInv.Origin.X, 14);
            Assert.Equal(-3.05361655856536, mInv.Origin.Y, 14);
            Assert.Equal(1.24081717884679, mInv.Origin.Z, 14);
        }

        [Fact]
        public void T05_ShouldCalculateInversed()
        {
            var m = new Coordinates3D(new Vector3D(-0.599949621335295, 0.467453495886029, 0.649267033695356),
                new Vector3D(-0.217217523080111, -0.876236386878922, 0.430146884187614),
                new Point3D(1.3789405517182, 4.29776658736997, -0.6070450812611));
            var mInv = m.Reversed;
            Assert.Equal(-0.599949621335295, mInv.X.X, 14);
            Assert.Equal(-0.217217523080111, mInv.X.Y, 14);
            Assert.Equal(0.769985064482797, mInv.X.Z, 14);
            Assert.Equal(0.467453495886029, mInv.Y.X, 14);
            Assert.Equal(-0.876236386878922, mInv.Y.Y, 14);
            Assert.Equal(0.11703428341004, mInv.Y.Z, 14);
            Assert.Equal(-0.787576794691292, mInv.Origin.X, 14);
            Assert.Equal(4.32650806755085, mInv.Origin.Y, 14);
            Assert.Equal(-1.18398866094702, mInv.Origin.Z, 14);
        }

        [Fact]
        public void T06_ShouldCalculateInversed()
        {
            var m = new Coordinates3D(new Vector3D(-0.285102753989341, 0.875750344712753, -0.389586644282356),
                new Vector3D(-0.95848022438309, -0.262887196601701, 0.110480683060163),
                new Point3D(-2.72196892542856, 2.15345893388309, -1.64915408783087));
            var mInv = m.Reversed;
            Assert.Equal(-0.285102753989341, mInv.X.X, 14);
            Assert.Equal(-0.95848022438309, mInv.X.Y, 14);
            Assert.Equal(-0.00566384447481441, mInv.X.Z, 14);
            Assert.Equal(0.875750344712753, mInv.Y.X, 14);
            Assert.Equal(-0.262887196601701, mInv.Y.Y, 14);
            Assert.Equal(0.404909441231484, mInv.Y.Z, 14);
            Assert.Equal(-3.30442164756852, mInv.Origin.X, 14);
            Assert.Equal(-1.86063693418814, mInv.Origin.Y, 14);
            Assert.Equal(0.62051365057054, mInv.Origin.Z, 14);
        }

        [Fact]
        public void T07_ShouldCalculateInversed()
        {
            var m = new Coordinates3D(new Vector3D(-0.597620097242217, 0.706857211246033, -0.378421857563335),
                new Vector3D(-0.603537802492208, -0.707305931787512, -0.368049507296337),
                new Point3D(-3.19451596503822, -3.0311283506598, -4.9095929460179));
            var mInv = m.Reversed;
            Assert.Equal(-0.597620097242217, mInv.X.X, 14);
            Assert.Equal(-0.603537802492208, mInv.X.Y, 14);
            Assert.Equal(-0.527818472900561, mInv.X.Z, 14);
            Assert.Equal(0.706857211246033, mInv.Y.X, 14);
            Assert.Equal(-0.707305931787511, mInv.Y.Y, 14);
            Assert.Equal(0.0084381139884076, mInv.Y.Z, 14);
            Assert.Equal(-1.62442929130372, mInv.Origin.X, 14);
#if FULLFRAMEWORK
            Assert.Equal(-5.87891947280388, mInv.Origin.Y, 14);
#else
            Assert.Equal(-5.87891947280387, mInv.Origin.Y, 14);
#endif
            Assert.Equal(2.50924481358202, mInv.Origin.Z, 14);
        }

        [Fact]
        public void T08_ShouldCalculateInversed()
        {
            var m = new Coordinates3D(new Vector3D(0.7474819852612, -0.649222564545007, 0.140644030785451),
                new Vector3D(0.037353108502252, -0.170309730205741, -0.984682355423548),
                new Point3D(0.215459743149326, -4.43718998666722, 1.97855274517953));
            var mInv = m.Reversed;
            Assert.Equal(0.747481985261201, mInv.X.X, 14);
            Assert.Equal(0.037353108502252, mInv.X.Y, 14);
            Assert.Equal(0.663231050988412, mInv.X.Z, 14);
            Assert.Equal(-0.649222564545007, mInv.Y.X, 14);
            Assert.Equal(-0.170309730205741, mInv.Y.Y, 14);
            Assert.Equal(0.741285813625792, mInv.Y.Z, 14);
            Assert.Equal(-3.32004777227431, mInv.Origin.X, 14);
            Assert.Equal(1.18450125678847, mInv.Origin.Y, 14);
            Assert.Equal(3.35022214287921, mInv.Origin.Z, 14);
        }

        [Fact]
        public void T09_ShouldCalculateInversed()
        {
            var m = new Coordinates3D(new Vector3D(0.571481566664062, -0.556630385708171, -0.602968848838447),
                new Vector3D(-0.819905187667347, -0.356753120429323, -0.447752938907289),
                new Point3D(-4.66189830082557, 3.63216113701098, -1.52678384749535));
            var mInv = m.Reversed;
            Assert.Equal(0.571481566664062, mInv.X.X, 14);
            Assert.Equal(-0.819905187667347, mInv.X.Y, 14);
            Assert.Equal(0.0341218727411386, mInv.X.Z, 14);
            Assert.Equal(-0.556630385708171, mInv.Y.X, 14);
            Assert.Equal(-0.356753120429323, mInv.Y.Y, 14);
            Assert.Equal(0.750259838169626, mInv.Y.Z, 14);
            Assert.Equal(3.76535710028357, mInv.Origin.X, 14);
            Assert.Equal(-3.21015173648591, mInv.Origin.Y, 14);
            Assert.Equal(-3.57406924184557, mInv.Origin.Z, 14);
        }

        [Fact]
        public void T10_ShouldCalculateInversed()
        {
            var m = new Coordinates3D(new Vector3D(-0.199757799583179, 0.725700580669313, -0.658373365744627),
                new Vector3D(0.923362791700238, -0.0854137021059747, -0.374306898675531),
                new Point3D(0.327651410050528, 3.9085582685231, -1.61201334400662));
            var mInv = m.Reversed;
            Assert.Equal(-0.199757799583179, mInv.X.X, 14);
            Assert.Equal(0.923362791700238, mInv.X.Y, 14);
            Assert.Equal(-0.327868840253582, mInv.X.Z, 14);
            Assert.Equal(0.725700580669313, mInv.Y.X, 14);
            Assert.Equal(-0.0854137021059747, mInv.Y.Y, 14);
            Assert.Equal(-0.682688191423269, mInv.Y.Z, 14);
            Assert.Equal(-3.83229873126393, mInv.Origin.X, 14);
            Assert.Equal(-0.572084404495988, mInv.Origin.Y, 14);
            Assert.Equal(1.72307169748944, mInv.Origin.Z, 14);
        }

        [Fact]
        public void T11_ShouldCalculateInversed()
        {
            var m = new Coordinates3D(new Vector3D(0.61441858731794, -0.38197848115536, -0.690349360463578),
                new Vector3D(0.660193983045746, -0.230196001335296, 0.714950142121417),
                new Point3D(2.36704916570664, 0.236999748850707, -2.85502251603409));
            var mInv = m.Reversed;
            Assert.Equal(0.61441858731794, mInv.X.X, 14);
            Assert.Equal(0.660193983045746, mInv.X.Y, 14);
            Assert.Equal(-0.432011231692442, mInv.X.Z, 14);
            Assert.Equal(-0.38197848115536, mInv.Y.X, 14);
            Assert.Equal(-0.230196001335296, mInv.Y.Y, 14);
            Assert.Equal(-0.895043150302534, mInv.Y.Z, 14);
            Assert.Equal(-3.33479316845864, mInv.Origin.X, 14);
            Assert.Equal(0.533043531328346, mInv.Origin.Y, 14);
            Assert.Equal(1.55089113679847, mInv.Origin.Z, 14);
        }

        [Fact]
        public void T12_ShouldCalculateInversed()
        {
            var m = new Coordinates3D(new Vector3D(-0.108764563334667, 0.965102800452131, 0.238215982507626),
                new Vector3D(-0.63057444541454, 0.118270180756929, -0.767064556040669),
                new Point3D(4.956503328847, -1.92766312366708, 1.24985909846139));
            var mInv = m.Reversed;
            Assert.Equal(-0.108764563334667, mInv.X.X, 14);
            Assert.Equal(-0.63057444541454, mInv.X.Y, 14);
            Assert.Equal(-0.768469998472787, mInv.X.Z, 14);
            Assert.Equal(0.965102800452131, mInv.Y.X, 14);
            Assert.Equal(0.118270180756929, mInv.Y.Y, 14);
            Assert.Equal(-0.233642352545889, mInv.Y.Z, 14);
            Assert.Equal(2.10174858607219, mInv.Origin.X, 14);
            Assert.Equal(4.31215201833227, mInv.Origin.Y, 14);
            Assert.Equal(2.61399234600267, mInv.Origin.Z, 14);
        }

        [Fact]
        public void T13_ShouldCalculateInversed()
        {
            var m = new Coordinates3D(new Vector3D(-0.471276394049064, -0.857457652286972, 0.206554919928394),
                new Vector3D(0.794765949244051, -0.514405701386767, -0.322077413525059),
                new Point3D(-3.4091664517341, 3.11294556041851, -2.78100840644027));
            var mInv = m.Reversed;
            Assert.Equal(-0.471276394049064, mInv.X.X, 14);
            Assert.Equal(0.794765949244051, mInv.X.Y, 14);
            Assert.Equal(0.38242077131651, mInv.X.Z, 14);
            Assert.Equal(-0.857457652286972, mInv.Y.X, 14);
            Assert.Equal(-0.514405701386767, mInv.Y.Y, 14);
            Assert.Equal(0.01237533495718, mInv.Y.Z, 14);
            Assert.Equal(1.63699028855978, mInv.Origin.X, 14);
            Assert.Equal(3.4151063609916, mInv.Origin.Y, 14);
            Assert.Equal(3.83460102914576, mInv.Origin.Z, 14);
        }

        [Fact]
        public void T14_ShouldCalculateInversed()
        {
            var m = new Coordinates3D(new Vector3D(0.616124902367323, 0.611384507589844, 0.496587443016817),
                new Vector3D(-0.218567701563095, -0.472998019502075, 0.853522719897099),
                new Point3D(3.66901853059838, 1.22928285795696, 3.0409801462856));
            var mInv = m.Reversed;
            Assert.Equal(0.616124902367323, mInv.X.X, 14);
            Assert.Equal(-0.21856770156309, mInv.X.Y, 14);
            Assert.Equal(0.756715444877586, mInv.X.Z, 14);
            Assert.Equal(0.611384507589844, mInv.Y.X, 14);
            Assert.Equal(-0.472998019502074, mInv.Y.Y, 14);
            Assert.Equal(-0.634414578510172, mInv.Y.Z, 14);
            Assert.Equal(-4.52225073385835, mInv.Origin.X, 14);
            Assert.Equal(-1.21216834116398, mInv.Origin.Y, 14);
            Assert.Equal(-1.51667062529796, mInv.Origin.Z, 14);
        }

        [Fact]
        public void T15_ShouldCalculateInversed()
        {
            var m = new Coordinates3D(new Vector3D(0.628436994926441, -0.6467093644797, 0.43224291931978),
                new Vector3D(-0.706524123204173, -0.242100993105561, 0.664989302521383),
                new Point3D(-4.01402559085471, 0.676306842675576, -4.36884140566403));
            var mInv = m.Reversed;
            Assert.Equal(0.628436994926441, mInv.X.X, 14);
            Assert.Equal(-0.706524123204173, mInv.X.Y, 14);
            Assert.Equal(-0.325408369189237, mInv.X.Z, 14);
            Assert.Equal(-0.6467093644797, mInv.Y.X, 14);
            Assert.Equal(-0.242100993105561, mInv.Y.Y, 14);
            Assert.Equal(-0.723293928518388, mInv.Y.Z, 14);
#if FULLFRAMEWORK
            Assert.Equal(4.84833691152391, mInv.Origin.X, 14);
#else
            Assert.Equal(4.84833691152392, mInv.Origin.X, 14);
#endif
            Assert.Equal(0.232961446337172, mInv.Origin.Y, 14);
            Assert.Equal(-3.47791974809742, mInv.Origin.Z, 14);
        }

        [Fact]
        public void T16_ShouldCalculateInversed()
        {
            var m = new Coordinates3D(new Vector3D(0.724081474064386, -0.537001942559217, -0.432822056513267),
                new Vector3D(0.659409328390014, 0.355015560605184, 0.662678873482788),
                new Point3D(3.62777816533473, -2.46061249983525, 2.93267248102123));
            var mInv = m.Reversed;
            Assert.Equal(0.724081474064386, mInv.X.X, 14);
            Assert.Equal(0.659409328390014, mInv.X.Y, 14);
            Assert.Equal(-0.202201277317865, mInv.X.Z, 14);
            Assert.Equal(-0.537001942559217, mInv.Y.X, 14);
            Assert.Equal(0.355015560605184, mInv.Y.Y, 14);
            Assert.Equal(-0.765240397140542, mInv.Y.Z, 14);
            Assert.Equal(-2.67883531951571, mInv.Origin.X, 14);
            Assert.Equal(-3.46205513350732, mInv.Origin.Y, 14);
            Assert.Equal(-2.94176337519435, mInv.Origin.Z, 14);
        }

        [Fact]
        public void T17_ShouldCalculateInversed()
        {
            var m = new Coordinates3D(new Vector3D(0.412168448937668, 0.775321842725172, 0.478532349892419),
                new Vector3D(0.657011072724381, -0.616806193313003, 0.433458844884127),
                new Point3D(-0.265450806014915, 2.96743442675445, 0.871235144264641));
            var mInv = m.Reversed;
            Assert.Equal(0.412168448937668, mInv.X.X, 14);
            Assert.Equal(0.657011072724381, mInv.X.Y, 14);
            Assert.Equal(0.631231827475354, mInv.X.Z, 14);
            Assert.Equal(0.775321842725172, mInv.Y.X, 14);
            Assert.Equal(-0.616806193313003, mInv.Y.Y, 14);
            Assert.Equal(0.135742992761933, mInv.Y.Z, 14);
            Assert.Equal(-2.60822048182677, mInv.Origin.X, 14);
            Assert.Equal(1.62709147223237, mInv.Origin.Y, 14);
            Assert.Equal(0.430047838352118, mInv.Origin.Z, 14);
        }

        [Fact]
        public void T18_ShouldCalculateInversed()
        {
            var m = new Coordinates3D(new Vector3D(-0.399721792643477, -0.358348146710997, 0.843687794290446),
                new Vector3D(-0.704972201843801, -0.468097963971068, -0.53282125591388),
                new Point3D(-3.02098086477303, -1.50084403646218, 4.5720579240341));
            var mInv = m.Reversed;
            Assert.Equal(-0.399721792643477, mInv.X.X, 14);
            Assert.Equal(-0.704972201843801, mInv.X.Y, 14);
            Assert.Equal(0.585864048319564, mInv.X.Z, 14);
            Assert.Equal(-0.358348146710997, mInv.Y.X, 14);
            Assert.Equal(-0.468097963971068, mInv.Y.Y, 14);
            Assert.Equal(-0.807756709582121, mInv.Y.Z, 14);
            Assert.Equal(-5.60276603107368, mInv.Origin.X, 14);
            Assert.Equal(-0.396159924478242, mInv.Origin.Y, 14);
            Assert.Equal(0.85711258485463, mInv.Origin.Z, 14);
        }

        [Fact]
        public void T19_ShouldCalculateInversed()
        {
            var m = new Coordinates3D(new Vector3D(-0.6718406001521, 0.590797149963861, 0.44675377511762),
                new Vector3D(0.724942189396914, 0.400744574367769, 0.560234422494017),
                new Point3D(-2.46370404374958, -1.14164620923886, 0.139439802216105));
            var mInv = m.Reversed;
            Assert.Equal(-0.6718406001521, mInv.X.X, 14);
            Assert.Equal(0.724942189396914, mInv.X.Y, 14);
            Assert.Equal(0.15195074866441, mInv.X.Z, 14);
            Assert.Equal(0.590797149963861, mInv.Y.X, 14);
            Assert.Equal(0.400744574367769, mInv.Y.Y, 14);
            Assert.Equal(0.700258890489349, mInv.Y.Z, 14);
            Assert.Equal(-1.04303033470621, mInv.Origin.X, 14);
            Assert.Equal(2.16543255063463, mInv.Origin.Y, 14);
            Assert.Equal(1.27107306250828, mInv.Origin.Z, 14);
        }

        [Fact]
        public void T20_ShouldCalculateInversed()
        {
            var m = new Coordinates3D(new Vector3D(0.552368916097507, 0.534658173017439, 0.639553921538223),
                new Vector3D(0.536255095242723, -0.815302911629452, 0.218429931819696),
                new Point3D(2.41726884498134, -3.74959011969603, 1.67968639949322));
            var mInv = m.Reversed;
            Assert.Equal(0.552368916097507, mInv.X.X, 14);
            Assert.Equal(0.536255095242723, mInv.X.Y, 14);
            Assert.Equal(0.638215522653189, mInv.X.Z, 14);
            Assert.Equal(0.534658173017439, mInv.Y.X, 14);
            Assert.Equal(-0.815302911629452, mInv.Y.Y, 14);
            Assert.Equal(0.222310144424839, mInv.Y.Z, 14);
            Assert.Equal(-0.404725192608, mInv.Origin.X, 14);
            Assert.Equal(-4.72021826241771, mInv.Origin.Y, 14);
            Assert.Equal(0.528865019535982, mInv.Origin.Z, 14);
        }
    }

#if ALLFEATURES
    internal class Coordinates3DGenerator
    {
        public static string GetCode(int nr)
        {
            var m = new Coordinates3D(RandomVector(), RandomVector(), RandomPoint());
            var s = new StringBuilder();
            s.AppendLine("[Fact]");
            s.AppendLine("public void T" + nr.ToString("00") + "_ShouldCalculateInversed()");
            s.AppendLine("{");
            s.Append("var m = new Coordinates3D(");
            s.Append(SerializeVector(m.X));
            s.AppendLine(",");
            s.Append(SerializeVector(m.Y));
            s.AppendLine(",");
            s.Append(SerializePoint(m.Origin));
            s.AppendLine(");");
            s.AppendLine(GetCode(GetReversed(m)));
            s.AppendLine("}");
            return s.ToString();
        }

        private static Coordinates3D GetReversed(Coordinates3D m1)
        {
            var m = m1.Matrix;
            m.Invert();
            return Coordinates3D.FromMatrix(ref m);
        }


        public static void GetCodeMany()
        {
            var sb = new StringBuilder();
            for (int i = 6; i <= 20; i++)
            {
                var code = GetCode(i);
                sb.AppendLine(code);
            }

            var bla = sb.ToString();
        }

        private static string GetCode(Coordinates3D expected)
        {
            var l = new List<string>();
            l.Add("var mInv = m.Reversed1Alt;");
            foreach (var s in "X,Y,Origin".Split(','))
            {
                var prop = expected.GetType().GetProperty(s);
                var value = prop.GetValue(expected);

                foreach (var c in "X,Y,Z".Split(','))
                {
                    var prop1 = value.GetType().GetProperty(c);
                    var value1 = (double)prop1.GetValue(value);
                    var name = $"mInv.{s}.{c}";
                    var assertEqual = string.Format("Assert.Equal({0}, {1}, 14);",
                        value1.ToString(CultureInfo.InvariantCulture),
                        name);
                    l.Add(assertEqual);
                }
            }

            return string.Join("\r\n", l);
            //var a1 = string.Join(", ", ToArray(expected).Select(a => a.ToString(CultureInfo.InvariantCulture)));
            // var a2 = string.Join(", ", ToArray(got).Select(a => a.ToString(CultureInfo.InvariantCulture)));
        }

        private static double RandomDouble() { return rnd.NextDouble() * 10 - 5; }

        private static Point3D RandomPoint() { return new Point3D(RandomDouble(), RandomDouble(), RandomDouble()); }

        private static Vector3D RandomVector() { return new Vector3D(RandomDouble(), RandomDouble(), RandomDouble()); }

        private static string SerializePoint(Point3D v)
        {
            return string.Format("new Point3D({0},{1},{2})", v.X.ToInv(), v.Y.ToInv(), v.Z.ToInv());
        }

        private static string SerializeVector(Vector3D v)
        {
            return string.Format("new Vector3D({0},{1},{2})", v.X.ToInv(), v.Y.ToInv(), v.Z.ToInv());
        }

        private static readonly Random rnd = new Random();
    }
#endif
}
