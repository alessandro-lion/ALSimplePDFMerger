using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ALSimplePDFMerger
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Metodo PDF Merge PER INCLUDERE ALLEGATI PDF STATICI
        public static void MergePDF(List<string> InFiles, string OutFile)
        {
            Document document = new Document();
            //create newFileStream object which will be disposed at the end
            using (FileStream newFileStream = new FileStream(OutFile, FileMode.Create))
            {
                PdfCopy writer = new PdfCopy(document, newFileStream);
                document.Open();

                foreach (string fileName in InFiles)
                {
                    PdfReader reader = new PdfReader(fileName);
                    reader.ConsolidateNamedDestinations();

                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        writer.AddPage(page);
                    }

                    //QUESTO CODICE IN TEORIA CONSENTE DI ACCODARE ALLEGATI CON FORM DI INSERIMENTO DATI PDF 
                    //*** NON TESTATO POTREBBE NON FUNZIONARE AFFATTO
                    //PRAcroForm form = reader.AcroForm;
                    //if (form != null)
                    //{
                    //    writer.CopyAcroForm(reader);
                    //}

                    reader.Close();
                }
                document.Close();
                writer.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog folderBrowserDialog1;
            string folderName;

            // Show the FolderBrowserDialog.
            folderBrowserDialog1 = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderName = folderBrowserDialog1.SelectedPath;
                textBoxFolder.Text = folderName;
                textBoxOutput.Text = folderName + "\\merged.pdf";
            }

            if (!RadioButtonF.Checked)
            {
                RadioButtonF.Checked = true;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool isOK = false;
            try
            {

                if (RadioButtonF.Checked)
                {
                    var files = Directory.EnumerateFiles(textBoxFolder.Text, "*.pdf").OrderBy(filename => filename);
                    MergePDF(files.ToList(), textBoxOutput.Text);
                }
                else
                {
                    //TODO: Merge from a copy and paste list within the richtextbox
                }

                isOK = true;
            }
            catch (Exception ex)
            {
                //MessageBox("Errore", ex.Message);
                var result = MessageBox.Show(ex.Message, "Errore",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Error);
            }
            finally
            {
                if (isOK)
                {
                    var result2 = MessageBox.Show("File " + textBoxOutput.Text + " saved. ", "Success",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Information);
                }
            }

        }
    }
}
