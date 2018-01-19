using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega_Sena
{
    class Program
    {
        //Realizando conexao com o banco de dados através da string.
        static string connectionString = "Data Source=USER-PC\\LOCALDB;Initial Catalog=Mega_Sena;Integrated Security=True";

        static void Main(string[] args)
        {
            string resultadoAposta = "";
            string resultadoMega = "";
            int[] aposta = new int[6];
            int[] resultado = new int[6];
            Random rnd = new Random();
            int acertos = 0;
            int op;
            int somadeacertos = 0;
            decimal dValorPago = 0;


            do
            {

                DateTime dataHora = DateTime.Now;


                //Menu do Console
                Console.Clear();
                Console.WriteLine("--------SISTEMA DE APOSTA DA MEGA SENA- SEFAZ-----------");
                Console.WriteLine("Data: " + dataHora);
                Console.WriteLine("QUANTIDADE DE ACERTOS: {0}", somadeacertos);
                Console.WriteLine();
                Console.WriteLine("[1] JOGAR");
                Console.WriteLine("[2] SAIR");
               

                op = Convert.ToInt32(Console.ReadLine());

                switch (op)
                {
                    case 1:
                        Console.Clear();
                        //Exibindo a data e a hora na aplicação
                        Console.WriteLine(" Hora: " + dataHora);
                        //Gera o numero do Concurso do jogo

                        int numero = rnd.Next(1, 1000);
                        Console.WriteLine("Numero do concurso: {0}", numero);
                        Console.WriteLine("FAÇA SUA APOSTA:");
                        int j = 0;
                        do
                        {


                            Console.WriteLine("Realize sua aposta, digitando o numero.");
                            Console.Write("Digite sua aposta: ");
                            int inputuser = Convert.ToInt32(Console.ReadLine());
                            if ((inputuser <= 60)) // verifica se o numero é menor ou igual a 60
                            {
                                if (!aposta.Contains(inputuser))
                                {
                                    aposta[j] = inputuser;
                                    j++; //só incremente se o numero for aceito
                                }
                                else
                                {
                                    Console.WriteLine("Número já apostado tente novamente.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Número digitado fora dos limites.");
                            }

                        } while (j < aposta.Length);//Sair quando o numero de apostas for satisfeito

                        Array.Sort(aposta);

                        Console.WriteLine();
                        Console.WriteLine("NÚMEROS APOSTADOS");
                        for (int i = 0; i < aposta.Length; i++)
                        {

                            if (!String.IsNullOrWhiteSpace(resultadoAposta))
                                resultadoAposta += "-";
                            resultadoAposta += aposta[i].ToString();
                        }
                        Console.WriteLine(resultadoAposta);

                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("RESULTADO DA MEGA-SENA");
                        for (int i = 0; i < resultado.Length; i++)
                        {
                            resultado[i] = rnd.Next(1, 60);
                        }
                        Array.Sort(resultado);

                        for (int i = 0; i < resultado.Length; i++)
                        {


                            if (!String.IsNullOrWhiteSpace(resultadoMega))
                                resultadoMega += "-";
                            resultadoMega += resultado[i].ToString();

                        }
                        Console.WriteLine(resultadoMega);

                        Console.WriteLine();
                        Console.WriteLine();
                        Console.Write("NÚMEROS ACERTADOS: ");
                        acertos = 0;
                        for (int i = 0; i < aposta.Length; i++)
                        {
                            if (aposta[i] == resultado[0] || aposta[i] == resultado[1] || aposta[i] == resultado[2] || aposta[i] == resultado[3] || aposta[i] == resultado[4] || aposta[i] == resultado[5])
                            {
                                acertos++;
                                Console.Write("{0:00} ", aposta[i]);
                            }
                        }
                        if (acertos == 0)
                            Console.WriteLine("NENHUM");

                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("QUANTIDADE DE ACERTOS: {0}", acertos);
                        Console.Write("VOCÊ GANHOU: ");
                        
                        switch (acertos)
                        {
                            case 1:
                                Console.WriteLine("R$ 0,00!");
                                dValorPago = 0;
                                Console.WriteLine("PESSIONE A TECLA ENTER PARA RETORNAR AO MENU PRINCIPAL!");
                                Console.WriteLine();
                                break;

                            case 4:
                                Console.Write("R$ 400,00!");
                                dValorPago = 400;
                                Console.WriteLine("Parabéns! Você acabou de acertar a quadra.");
                                break;

                            case 5:
                                Console.WriteLine("R$ 500.000,00!");
                                dValorPago = 50000000;
                                Console.WriteLine("Parabéns! Você acabou de acertar a quina.");
                                break;

                            case 6:
                                Console.WriteLine("R$ 12.000.000,00!");
                                dValorPago = 1200000000;
                                Console.WriteLine("Parabéns! Você acabou de acertar a sena.");
                                break;

                            default:
                                Console.WriteLine("R$ 0,00.");
                                Console.WriteLine("PRESSIONE ENTER PARA RETORNAR AO MENU PRINCIPAL.....");
                                dValorPago = 0;
                                break;
                        }

                        int idConcurso = InserirTbConcurso(dataHora, dValorPago, resultadoMega, 0, 0, 0, 0, 0, 0);
                        if (idConcurso > 0)
                        {
                            InserirTbAposta(idConcurso, dataHora, resultadoAposta);
                        }

                        somadeacertos += acertos;
                        Console.ReadKey();
                        Console.Clear();
                        break;

                    case 2:
                        Console.Clear();
                        Console.WriteLine("PRESSIONE ENTER PARA SAIR DO SISTEMA DE APOSTAS.....");
                        Console.ReadKey();
                        break;

                    default:
                        Console.WriteLine("OPÇÃO INVÁLIDA. TENTE NOVAMENTE.");
                        System.Threading.Thread.Sleep(2000);
                        break;
                }

            } while (op != 2);

        }



        //Inserindo informações do concurso no banco de dados
        static int InserirTbConcurso(DateTime dt_concurso, decimal valor_pago, string numeros_sorteados, int ganhadores_sena, decimal premiacao_sena, int ganhadores_quina, decimal premiacao_quina, int ganhadores_quadra, decimal premiacao_quadra)
        {
            StringBuilder sbTbConcurso = new StringBuilder();
            sbTbConcurso.Append("INSERT INTO [dbo].[tb_Concurso]");
            sbTbConcurso.Append(" (dt_concurso, valor_pago, numeros_sorteados, ganhadores_sena, premiacao_sena, ganhadores_quina, premiacao_quina, ganhadores_quadra, premiacao_quadra)");
            sbTbConcurso.Append(" VALUES");
            sbTbConcurso.Append(" (@dt_concurso, @valor_pago, @numeros_sorteados, @ganhadores_sena, @premiacao_sena, @ganhadores_quina, @premiacao_quina, @ganhadores_quadra, @premiacao_quadra);");
            sbTbConcurso.Append(" SELECT CAST(scope_identity() AS int);");

            try
            {
                SqlConnection conexao = new SqlConnection();
                conexao.ConnectionString = connectionString;
                conexao.Open();

                using (SqlCommand cmd = new SqlCommand(sbTbConcurso.ToString(), conexao))
                {
                    cmd.Parameters.Add(new SqlParameter("dt_concurso", dt_concurso));
                    cmd.Parameters.Add(new SqlParameter("valor_pago", valor_pago));
                    cmd.Parameters.Add(new SqlParameter("numeros_sorteados", numeros_sorteados));
                    cmd.Parameters.Add(new SqlParameter("ganhadores_sena", ganhadores_sena));
                    cmd.Parameters.Add(new SqlParameter("premiacao_sena", premiacao_sena));
                    cmd.Parameters.Add(new SqlParameter("ganhadores_quina", ganhadores_quina));
                    cmd.Parameters.Add(new SqlParameter("premiacao_quina", premiacao_quina));
                    cmd.Parameters.Add(new SqlParameter("ganhadores_quadra", ganhadores_quadra));
                    cmd.Parameters.Add(new SqlParameter("premiacao_quadra", premiacao_quadra));

                    return (Int32)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static void InserirTbAposta(int id_concurso, DateTime data_hora, string numeros_apostados)
        {
            StringBuilder sbTbConcurso = new StringBuilder();
            sbTbConcurso.Append("INSERT INTO [dbo].[tb_Aposta]");
            sbTbConcurso.Append(" (id_concurso, data_hora, numeros_apostados)");
            sbTbConcurso.Append(" VALUES");
            sbTbConcurso.Append(" (@id_concurso, @data_hora, @numeros_apostados);");

            try
            {
                SqlConnection conexao = new SqlConnection();
                conexao.ConnectionString = connectionString;
                conexao.Open();

                using (SqlCommand cmd = new SqlCommand(sbTbConcurso.ToString(), conexao))
                {
                    cmd.Parameters.Add(new SqlParameter("id_concurso", id_concurso));
                    cmd.Parameters.Add(new SqlParameter("data_hora", data_hora));
                    cmd.Parameters.Add(new SqlParameter("numeros_apostados", numeros_apostados));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        
    }
}
