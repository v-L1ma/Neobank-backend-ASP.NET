namespace Neobank.UseCases;

public class CriarChavePixUseCase : ICriarChavePixUseCase
{

  private readonly AppDbContext _context;

  public CriarChavePixUseCase(AppDbContext context){
    _context = context;
  }

  public async Task Criar(CriarChavePixDto dto){      

      if (dto.ClienteId.IsNullOrEmpty() || dto.Chave.IsNullOrEmpty() || dto.Tipo.IsNullOrEmpty())
          {
              throw new Exception("Todos os campos são obrigatórios.");
          }
  
          var cliente = await _context.Users.FindAsync(dto.ClienteId);
  
          if (cliente is null)
          {
              throw new Exception("Conta não encontrada.");
          }
  
          if (_context.ChavesPix.Any(chave => chave.ClienteId == cliente.Id && chave.Tipo == dto.Tipo ) )
          {
              throw new Exception($"Já existe uma chave do tipo {dto.Tipo}.");
          }
  
          ChavePix novaChave;
  
          switch (dto.Tipo)
          {
              case "Celular":
                  novaChave = new ChavePix
                  {
                      ClienteId = dto.ClienteId,
                      Tipo = "Celular",
                      Chave = dto.Chave
                  };
                  break;
              case "CPF":
                  novaChave = new ChavePix
                  {
                      ClienteId = dto.ClienteId,
                      Tipo = "CPF",
                      Chave = dto.Chave
                  };
                  break;
              case "Email":
                  novaChave = new ChavePix
                  {
                      ClienteId = dto.ClienteId,
                      Tipo = "Email",
                      Chave = dto.Chave
                  };
                  break;
              case "Aleatória":
                  novaChave = new ChavePix
                  {
                      ClienteId = dto.ClienteId,
                      Tipo = "Aleatória",
                      Chave = Guid.NewGuid().ToString()
                  };
                  break;
              default:
                  throw new Exception("Por favor insira um tipo de chave PIX válido.");
          }
  
          _context.ChavesPix.Add(novaChave);
          await _context.SaveChangesAsync();

          return novaChave;
        }
      }
