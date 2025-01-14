﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace mercearia_seu_joao.View
{
    public partial class frmGerenciarUsuario : Window
    {
        List<Usuario> listaDeUsuarios = new List<Usuario>();

        public frmGerenciarUsuario()
        {
            InitializeComponent();
            cbTipoUsuario.Items.Add("Caixa");
            cbTipoUsuario.Items.Add("Gerente");
            AtualizaDataGrid();
        }
        private bool ValidarSenha(string senha)
        {
            string especiais = "!@#$%¨&*()_+-=}{´`ªº][^~|?,<>/°";
            string letras = "qwertyuiopasdfghjklçzxcvbnm";
            string maiusculas = letras.ToUpper();
            string numeros = "1234567890";
            int tamanhoMinimo = 8;

            bool isNumeroValidos = false;
            bool isLetrasValidas = false;
            bool isMaiusculas = false;
            bool isCaracteresEspeciaisValidos = false;
            bool isTamanhoMinimoValido = false;

            if (senha.Length <= tamanhoMinimo)
            {
                isTamanhoMinimoValido = true;
                for (int i = 0; i < senha.Length; i++)
                {
                    if (isNumeroValidos == false)
                    {
                        foreach (char c in numeros)
                        {
                            if (c == senha[i])
                            {
                                isNumeroValidos = true;
                                break;
                            }
                        }
                    }

                    if (isLetrasValidas == false)
                    {
                        foreach (char c in letras)
                        {
                            if (c.ToString() == senha[i].ToString())
                            {
                                isLetrasValidas = true;
                                break;
                            }
                        }
                    }
                    if (isMaiusculas == false)
                    {
                        foreach (char c in maiusculas)
                        {
                            if (c.ToString() == senha[i].ToString())
                            {
                                isMaiusculas = true;
                                break;
                            }
                        }
                    }

                    if (isCaracteresEspeciaisValidos == false)
                    {
                        foreach (char c in especiais)
                        {
                            if (c == senha[i])
                            {
                                isCaracteresEspeciaisValidos = true;
                                break;
                            }
                        }
                    }
                }
            }
            if (isNumeroValidos == true && isLetrasValidas == true && isCaracteresEspeciaisValidos == true && isTamanhoMinimoValido == true && isMaiusculas == true)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
        private void AdicionarUsuario()
        {
            string data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            bool foiInserido = ConsultasUsuario.NovoUsuario(boxEmail.Text, boxSenha.Password, boxNome.Text, data, cbTipoUsuario.Text);
            if (foiInserido == true)
            {
                if (boxSenha.Password == boxConfirmarSenha.Password)
                {
                    Mensagens.ExibirMensagemUsuarioAdicionado();
                }
                else
                {
                    Mensagens.ExibirMensagemErroUsuarioCadastrado();
                }
            }
        }
        private void AtualizaDataGrid()
        {
            listaDeUsuarios.Clear();
            listaDeUsuarios = cUsuario.ObterTodosUsuarios();
            dbUsuario.ItemsSource = listaDeUsuarios;
            dbUsuario.Items.Refresh();
        }
        private bool VerificaCampos()
        {
            if (boxNome.Text != "" && boxEmail.Text != "" && boxConfirmarSenha.Password != "" && boxSenha.Password != "" && cbTipoUsuario.Text != "")
            {
                return true;
            }
            else
            {
                Mensagens.MensagemPreenchaTodosCampos();
                return false;
            }
        }
        private void LimpaTodosOsCampos()
        {
            boxId.Text = "";
            boxNome.Text = "";
            boxConfirmarSenha.Password = "";
            boxSenha.Password = "";
            boxEmail.Text = "";
            cbTipoUsuario.Text = "";
        }

        private void Novo(object sender, RoutedEventArgs e)
        {
            if (VerificaCampos() == true)
            {
                if (ValidarSenha(boxSenha.Password) == true)
                {
                    AdicionarUsuario();
                    AtualizaDataGrid();
                }
            }
        }

        private void AtualizarUsuario(object sender, RoutedEventArgs e)
        {
            string data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (VerificaCampos() == true)
            {
                if (boxId.Text != "")
                {
                    int id = int.Parse(boxId.Text);
                    MessageBoxResult result = MessageBox.Show(
                        $"Deseja alterar o usuário id:{id} ?",
                        "Alterar Usuário",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        bool foiAtualizado = cUsuario.AlterarUsuario(
                            id,
                            boxNome.Text,
                            boxEmail.Text,
                            boxSenha.Password,
                            data,
                            cbTipoUsuario.Text
                            );

                        if (foiAtualizado == true)
                        {
                            Mensagens.ExibirMensagemUsuarioAtualizado();
                            LimpaTodosOsCampos();
                            AtualizaDataGrid();
                        }
                        else
                        {
                            Mensagens.ExibirMensagemErroUsuarioAtualizado();
                        }
                    }
                }
            }
        }
        private void dbEUsuario(object sender, MouseButtonEventArgs e)
        {
            Usuario usuario = (Usuario)dbUsuario.SelectedItem;
            boxId.Text = usuario.id.ToString();
            boxNome.Text = usuario.nome;
            boxEmail.Text = usuario.email;
            boxSenha.Password = usuario.senha;
        }

        private void BotaoDeLimpar(object sender, RoutedEventArgs e)
        {
            LimpaTodosOsCampos();
        }

        private void Excluir(object sender, RoutedEventArgs e)
        {
            if (VerificaCampos() == true)
            {
                string data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if (txtId.Text != "")
                {
                    int id = int.Parse(boxId.Text);
                    MessageBoxResult result = MessageBox.Show(
                        $"Deseja excluir o usuario id:{id} ?",
                        "Excluir Usuario",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        bool foiExcluido = cUsuario.ExcluirUsuario(data);
                        if (foiExcluido == true)
                        {
                            Mensagens.ExibirMensagemUsuarioExcluido();
                            LimpaTodosOsCampos();
                            AtualizaDataGrid();
                        }
                        else
                        {
                            Mensagens.ExibirMensagemErroUsuarioExcluido();
                        }
                    }
                }
                AtualizaDataGrid();
            }
        }
    }
}
