using System;
using System.Diagnostics;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace Etiquetadora
{
    class Reporte
    {
        string[] datos;
        string path = @"C:\Etiquetas";
        static int MAX_ARCH = 5; 

        public Reporte(string[] datos)
        {
            this.datos = datos;
        }

        public void creaDirectorio()
        {
            try
            {
                if(! Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception e) { }
        }

        public int totalArchivos()
        {
            int num_arch = 0;
            DirectoryInfo di = new DirectoryInfo(path);

            foreach (var fi in di.GetFiles())
            {
                num_arch = num_arch + 1;
            }
            return num_arch;
        }

        public void borraArchivos()
        {
            try
            {
                if (Directory.Exists(path))
                {
                    if (totalArchivos() >= MAX_ARCH)
                    {
                        DirectoryInfo di = new DirectoryInfo(path);
                        foreach (var fichero in di.GetFiles("*.pdf"))
                        {
                            fichero.Delete();
                        }
                    }
                }
            }
            catch (Exception e) { }
        }

        public bool generaEtiqueta()
        {
            creaDirectorio();
            try
            {
                string filename = path + @"\" + datos[1] + ".pdf";
                Document doc = new Document(new Rectangle(286.30f, 357.16f), 0.2f, 0.2f, 0, 0);
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filename, FileMode.Create));
                writer.PageEvent = new itsEvents(datos[18], datos[19], datos[20], datos[21]);

                doc.Open();
                iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font _FontBold = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font _FontDatos = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font _FontBold2 = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 13.5f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font _FontBold3 = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 11.5f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font _FontBold4 = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 18.5f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font _FontBold5 = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 10.00f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

                PdfPTable tblPrueba = new PdfPTable(2);
                tblPrueba.WidthPercentage = 100;

                PdfPCell clPais = new PdfPCell(new Phrase("Orden de Embarque", _FontBold3));
                clPais.BorderWidth = 0.0f;
                clPais.Colspan = 2;
                clPais.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(@"C:\img_etiq\etiquetas.png");
                imagen.BorderWidth = 0;
                imagen.Alignment = Element.ALIGN_CENTER;
                imagen.ScaleAbsolute(170, 50);

                PdfPCell clNombre = new PdfPCell(imagen);
                clNombre.BorderWidth = 0.0f;
                clNombre.PaddingTop = 2;
                clNombre.Rowspan = 2;

                PdfPCell clApellido = new PdfPCell(new Phrase("PED: " + datos[0], _FontBold2));
                clApellido.BorderWidth = 0.0f;
                clApellido.PaddingTop = 2;
                clApellido.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                PdfPCell clFolio = new PdfPCell(new Phrase("FOL: " + datos[1].Trim(), _FontBold2));
                clFolio.BorderWidth = 0.0f;
                clFolio.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                //PdfPCell clAgente = new PdfPCell(new Phrase("Datos del Agente", _FontBold3));
                PdfPCell clAgente = new PdfPCell(new Phrase("Datos del Cliente", _FontBold3));
                clAgente.BorderWidth = 0.0f;
                clAgente.BorderWidthTop = 0.0f;
                clAgente.PaddingLeft = 10;
                clAgente.Colspan = 2;
                clAgente.PaddingTop = 2;
                clAgente.PaddingBottom = 2;

                //PdfPCell clInfoAgnt = new PdfPCell(new Phrase(datos[2], _FontBold5));
                PdfPCell clInfoAgnt = new PdfPCell(new Phrase(datos[5], _FontBold2));
                clInfoAgnt.BorderWidth = 1.00f;
                clInfoAgnt.BorderWidthBottom = 0.0f;
                clInfoAgnt.Colspan = 2;
                clInfoAgnt.PaddingTop = 2;
                clInfoAgnt.PaddingBottom = 2;
                clInfoAgnt.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                clInfoAgnt.BorderWidthLeft = 0.0f;
                clInfoAgnt.BorderWidthRight = 0.0f;

                PdfPCell clAgenteRuta = new PdfPCell(new Phrase(" " + datos[3], _FontBold2));
                clAgenteRuta.BorderWidth = 0.00f;
                clAgenteRuta.BorderWidthTop = 0.0f;
                clAgenteRuta.Colspan = 2;
                clAgenteRuta.PaddingTop = 2;
                clAgenteRuta.PaddingBottom = 2;
                clAgenteRuta.PaddingLeft = 20;
                clAgenteRuta.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                clAgenteRuta.BorderWidthLeft = 0.0f;
                clAgenteRuta.BorderWidthRight = 0.0f;

                PdfPCell clCveCte = new PdfPCell(new Phrase("  " + datos[4], _FontBold5));
                clCveCte.Colspan = 2;
                clCveCte.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                clCveCte.BorderWidthLeft = 0.0f;
                clCveCte.BorderWidthRight = 0.0f;
                clCveCte.BorderWidth = 0.00f;
                clCveCte.BorderWidthTop = 1.0f;
                clCveCte.BorderWidthBottom = 0.0f;

                PdfPCell clDiCte = new PdfPCell(new Phrase(datos[6] + " Num: " + datos[8] + ", Int: " + datos[7] + ", " + datos[9] + " " + datos[10], _FontBold5));
                clDiCte.Colspan = 2;
                clDiCte.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                clDiCte.BorderWidthLeft = 0.0f;
                clDiCte.BorderWidthRight = 0.0f;
                clDiCte.BorderWidth = 0.00f;
                clDiCte.BorderWidthTop = 1.0f;
                clDiCte.BorderWidthBottom = 1.0f;

                //PdfPCell clClte = new PdfPCell(new Phrase("Datos del Cliente", _FontBold));
                PdfPCell clClte = new PdfPCell(new Phrase(""));
                clClte.BorderWidthTop = 0.0f;
                clClte.Colspan = 2;
                clClte.PaddingTop = 0;
                clClte.PaddingLeft = 10;
                clClte.PaddingBottom = 0;

                PdfPCell clInfoCte = new PdfPCell();
                clInfoCte.BorderWidth = 0;
                clInfoCte.Colspan = 2;
                clInfoCte.PaddingBottom = 2;

                PdfPTable tabla_Cte = new PdfPTable(2);
                tabla_Cte.WidthPercentage = 100;

                /*PdfPCell clCveCte = new PdfPCell(new Phrase(datos[4], _FontBold5));
                clCveCte.Colspan = 2;
                clCveCte.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                clCveCte.BorderWidthLeft = 0.0f;
                clCveCte.BorderWidthRight = 0.0f;*/

                //PdfPCell clNomCte = new PdfPCell(new Phrase(datos[5], _FontBold5));
                PdfPCell clNomCte = new PdfPCell(new Phrase());
                clNomCte.Colspan = 2;
                clNomCte.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                clNomCte.BorderWidthBottom = 0.0f;
                clNomCte.BorderWidthLeft = 0.0f;
                clNomCte.BorderWidthRight = 0.0f;
                clNomCte.BorderWidthTop = 0.0f;

                PdfPCell clCalle = new PdfPCell(new Phrase("Datos del Agente", _FontBold3));
                clCalle.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                clCalle.BorderWidth = 0.0f;
                clCalle.BorderWidthBottom = 1.00f;
                clCalle.PaddingTop = 1;

                PdfPCell clDatos = new PdfPCell();
                clDatos.Rowspan = 4;
                clDatos.BorderWidthRight = 0.0f;
                clDatos.BorderWidthLeft = 1.0f;
                clDatos.BorderWidthBottom = 1.0f;
                clDatos.BorderWidthTop = 0.0f;

                PdfPTable tab_Datos = new PdfPTable(2);
                tab_Datos.WidthPercentage = 100;

                PdfPCell cel_Hiel = new PdfPCell(new Phrase("Hiel: ", _standardFont));
                PdfPCell celDat_Hiel = new PdfPCell(new Phrase(datos[23], _FontDatos));
                cel_Hiel.BorderWidth = 0.0f;
                cel_Hiel.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                celDat_Hiel.BorderWidth = 0.0f;
                cel_Hiel.BorderWidthRight = 1.0f;

                PdfPCell cel_Lab = new PdfPCell(new Phrase("Lab: ", _standardFont));
                PdfPCell celDat_Lab = new PdfPCell(new Phrase(datos[11], _FontDatos));
                cel_Lab.BorderWidth = 0.0f;
                cel_Lab.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                celDat_Lab.BorderWidth = 0.0f;
                cel_Lab.BorderWidthRight = 1.0f;

                PdfPCell cel_C0 = new PdfPCell(new Phrase("00: ", _standardFont));
                PdfPCell celDat_C0 = new PdfPCell(new Phrase(datos[12], _FontDatos));
                cel_C0.BorderWidth = 0.0f;
                cel_C0.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                celDat_C0.BorderWidth = 0.0f;
                cel_C0.BorderWidthRight = 1.0f;

                PdfPCell cel_C1 = new PdfPCell(new Phrase("01: ", _standardFont));
                PdfPCell celDat_C1 = new PdfPCell(new Phrase(datos[13], _FontDatos));
                cel_C1.BorderWidth = 0.0f;
                cel_C1.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                celDat_C1.BorderWidth = 0.0f;
                cel_C1.BorderWidthRight = 1.0f;

                PdfPCell cel_C2 = new PdfPCell(new Phrase("02: ", _standardFont));
                PdfPCell celDat_C2 = new PdfPCell(new Phrase(datos[14], _FontDatos));
                cel_C2.BorderWidth = 0.0f;
                cel_C2.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                celDat_C2.BorderWidth = 0.0f;
                cel_C2.BorderWidthRight = 1.0f;

                PdfPCell cel_C3 = new PdfPCell(new Phrase("03: ", _standardFont));
                PdfPCell celDat_C3 = new PdfPCell(new Phrase(datos[15], _FontDatos));
                cel_C3.BorderWidth = 0.0f;
                cel_C3.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                celDat_C3.BorderWidth = 0.0f;
                cel_C3.BorderWidthRight = 1.0f;

                PdfPCell cel_C4 = new PdfPCell(new Phrase("04: ", _standardFont));
                PdfPCell celDat_C4 = new PdfPCell(new Phrase(datos[16], _FontDatos));
                cel_C4.BorderWidth = 0.0f;
                cel_C4.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                celDat_C4.BorderWidth = 0.0f;
                cel_C4.BorderWidthRight = 1.0f;

                PdfPCell cel_Tot = new PdfPCell(new Phrase("Total: " + datos[17], _FontBold4));
                cel_Tot.Colspan = 2;
                cel_Tot.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                cel_Tot.BorderWidth = 0.0f;

                tab_Datos.AddCell(cel_Hiel);
                tab_Datos.AddCell(celDat_Hiel);
                tab_Datos.AddCell(cel_Lab);
                tab_Datos.AddCell(celDat_Lab);
                tab_Datos.AddCell(cel_C0);
                tab_Datos.AddCell(celDat_C0);
                tab_Datos.AddCell(cel_C1);
                tab_Datos.AddCell(celDat_C1);
                tab_Datos.AddCell(cel_C2);
                tab_Datos.AddCell(celDat_C2);
                tab_Datos.AddCell(cel_C3);
                tab_Datos.AddCell(celDat_C3);
                tab_Datos.AddCell(cel_C4);
                tab_Datos.AddCell(celDat_C4);
                tab_Datos.AddCell(cel_Tot);
                clDatos.AddElement(tab_Datos);

                PdfPCell clNum = new PdfPCell(new Phrase("RUTA: " + datos[3], _FontBold));
                clNum.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                clNum.PaddingRight = 10;
                clNum.BorderWidth = 0.0f;
                clNum.PaddingTop = 5;

                PdfPCell clColonia = new PdfPCell(new Phrase("AGENTE: " + datos[2], _FontBold));
                clColonia.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                clColonia.BorderWidthTop = 0.0f;
                clColonia.BorderWidthLeft = 0.0f;
                clColonia.BorderWidthRight = 0.0f;
                clColonia.BorderWidthBottom = 1.0f;

                //tabla_Cte.AddCell(clCveCte);

                PdfPCell clObs = new PdfPCell(new Phrase("OBS: " + datos[22], _FontBold));
                clObs.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                clObs.BorderWidthTop = 0.0f;
                clObs.BorderWidthLeft = 0.0f;
                clObs.BorderWidthRight = 0.0f;
                clObs.BorderWidthBottom = 1.0f;

                tabla_Cte.AddCell(clNomCte);
                tabla_Cte.AddCell(clCalle);
                tabla_Cte.AddCell(clDatos);
                tabla_Cte.AddCell(clNum);
                tabla_Cte.AddCell(clColonia);
                tabla_Cte.AddCell(clObs);
                clInfoCte.AddElement(tabla_Cte);

                tblPrueba.AddCell(clPais);
                tblPrueba.AddCell(clNombre);
                tblPrueba.AddCell(clApellido);
                tblPrueba.AddCell(clFolio);
                tblPrueba.AddCell(clAgente); 
                tblPrueba.AddCell(clInfoAgnt);
                //tblPrueba.AddCell(clAgenteRuta);
                tblPrueba.AddCell(clCveCte); /**/
                tblPrueba.AddCell(clDiCte); /**/
                tblPrueba.AddCell(clClte);
                tblPrueba.AddCell(clInfoCte);

                doc.Add(tblPrueba);
                doc.Close();
                writer.Close();

                Process proc = new Process();
                proc.StartInfo.FileName = filename;
                proc.Start();
                proc.Close();

                return true;
            }
            catch (Exception e) { return false;  }
        }

        public class itsEvents : PdfPageEventHelper
        {
            PdfContentByte cbp, cbp1, cbp2, cbpF;
            string fecha;
            string dcto;
            string empco;
            string fcia;

            public itsEvents(string fecha, string dcto, string empco, string fcia)
            {
                this.fecha = fecha;
                this.dcto = dcto;
                this.empco = empco;
                this.fcia = fcia;
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);
                cbp = writer.DirectContent;
                cbp1 = writer.DirectContent;
                cbp2 = writer.DirectContent;
                cbpF = writer.DirectContent;

                cbp.BeginText();
                cbp.SetFontAndSize(FontFactory.GetFont(FontFactory.HELVETICA_BOLD, iTextSharp.text.Font.BOLD).BaseFont, 10);
                cbp.SetColorFill(iTextSharp.text.BaseColor.BLACK);
                cbp.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, fecha, 280, 5, 0);
                cbp.EndText();

                cbpF.BeginText();
                cbpF.SetFontAndSize(FontFactory.GetFont(FontFactory.HELVETICA_BOLD, iTextSharp.text.Font.NORMAL).BaseFont, 13);
                cbpF.SetColorFill(iTextSharp.text.BaseColor.BLACK);
                cbpF.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "  " + fcia, 0, 45, 0);
                cbpF.EndText();

                cbp2.BeginText();
                cbp2.SetFontAndSize(FontFactory.GetFont(FontFactory.HELVETICA_BOLD, iTextSharp.text.Font.NORMAL).BaseFont, 10);
                cbp2.SetColorFill(iTextSharp.text.BaseColor.BLACK);
                cbp2.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "  Documentó: ", 0, 26, 0);
                cbp2.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "  Empacó: ", 0, 11, 0);
                cbp2.EndText();

                cbp1.BeginText();
                cbp1.SetFontAndSize(FontFactory.GetFont(FontFactory.HELVETICA_BOLD, iTextSharp.text.Font.BOLD).BaseFont, 12);
                cbp1.ShowTextAligned(PdfContentByte.ALIGN_LEFT, dcto, 64, 25, 0);
                cbp1.ShowTextAligned(PdfContentByte.ALIGN_LEFT, empco, 48, 10, 0);
                cbp1.EndText();
            }
        }
    }
}