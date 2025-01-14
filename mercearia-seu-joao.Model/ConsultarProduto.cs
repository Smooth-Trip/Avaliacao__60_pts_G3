﻿using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ConsultarProduto
{
    public static bool InserirProduto(string nome, int qtdEstoque, float precoUnitario, string fornecedor, string dataInserido)
    {
        var conexao = new MySqlConnection(ConexaoBD.Connection.ConnectionString);
        bool foiInserido = false;

        try
        {
            conexao.Open();
            var comando = conexao.CreateCommand();
            comando.CommandText = @"
                INSERT INTO produto (nome, qtdEstoque, precoUnitario, fornecedor, dataInserido, dataExcluido = null) 
                VALUES (@nome, @qtdEstoque, @precoUnitario, @fornecedor, @dataInserido)";
            comando.Parameters.AddWithValue("@nome", nome);
            comando.Parameters.AddWithValue("@qtdEstoque", qtdEstoque);
            comando.Parameters.AddWithValue("precoUnitario", precoUnitario);
            comando.Parameters.AddWithValue("@fornecedor", fornecedor);
            comando.Parameters.AddWithValue("@dataInserido", dataInserido);
            var leitura = comando.ExecuteReader();
            foiInserido = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            if (conexao.State == System.Data.ConnectionState.Open)
            {
                conexao.Close();
            }
        }
        return foiInserido;
    }

    public static bool ExcluirProduto(int id, string dataExcluido)
    {
        var conexao = new MySqlConnection(ConexaoBD.Connection.ConnectionString);
        bool foiExcluido = false;

        try
        {
            conexao.Open();
            var comando = conexao.CreateCommand();
            comando.CommandText = @"
                INSERT INTO Produto (dataExcluido) 
                VALUES (@dataExcluido)

                WHERE id = @id";
            comando.Parameters.AddWithValue("@id", id);
            comando.Parameters.AddWithValue("@dataExcluido", dataExcluido);
            var leitura = comando.ExecuteReader();
            foiExcluido = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            if (conexao.State == System.Data.ConnectionState.Open)
            {
                conexao.Close();
            }
        }
        return foiExcluido;
    }

    public static bool AlterarProduto(int id, string nome, int qtdEstoque, float precoUnitario, string fornecedor, string dataAlterado)
    {
        var conexao = new MySqlConnection(ConexaoBD.Connection.ConnectionString);
        bool foiAtualizado = false;

        try
        {
            conexao.Open();
            var comando = conexao.CreateCommand();
            comando.CommandText = @"
                UPDATE Produto 
                SET nome @nome, qtdEstoque @qtdEstoque, precoUnitario @precoUnitario, fornecedor @fornecedor dataAlterado @dataAlterado, dataExcluido = null
                WHERE id = @id";
            comando.Parameters.AddWithValue("@id", id);
            comando.Parameters.AddWithValue("@nome", nome);
            comando.Parameters.AddWithValue("@qtdEstoque", qtdEstoque);
            comando.Parameters.AddWithValue("@precoUnitario", precoUnitario);
            comando.Parameters.AddWithValue("@fornecedor", fornecedor);
            comando.Parameters.AddWithValue("@dataAlterado", dataAlterado);
            var leitura = comando.ExecuteReader();
            foiAtualizado = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            if (conexao.State == System.Data.ConnectionState.Open)
            {
                conexao.Close();
            }
        }
        return foiAtualizado;
    }
    public static List<Produto> ObterTodosProdutos()
    {
        var conexao = new MySqlConnection(ConexaoBD.Connection.ConnectionString);
        List<Produto> listaDeProdutos = new List<Produto>();

        try
        {
            conexao.Open();
            var comando = conexao.CreateCommand();
            comando.CommandText = @"
                SELECT id, nome, precoUnitario, qtdEstoque, fornecedor FROM Produto 
                WHERE dataExcluido = null";
            var leitura = comando.ExecuteReader();
            while (leitura.Read())
            {
                Produto produto = new Produto();
                produto.id = leitura.GetInt32("id");
                produto.nome = leitura.GetString("nome");
                produto.precoUnitario = (float)leitura.GetDecimal("precoUnitario");
                produto.qtdEstoque = leitura.GetInt32("qtdEstoque");
                produto.fornecedor = leitura.GetString("fornecedor");


                listaDeProdutos.Add(produto);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            if (conexao.State == System.Data.ConnectionState.Open)
            {
                conexao.Close();
            }
        }
        return listaDeProdutos;
    }
}
