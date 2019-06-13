namespace TaskManagerApplication
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class TaskManagerContext : DbContext
    {
        // Контекст настроен для использования строки подключения "TaskManagerContext" из файла конфигурации  
        // приложения (App.config или Web.config). По умолчанию эта строка подключения указывает на базу данных 
        // "TaskManagerApplication.TaskManagerContext" в экземпляре LocalDb. 
        // 
        // Если требуется выбрать другую базу данных или поставщик базы данных, измените строку подключения "TaskManagerContext" 
        // в файле конфигурации приложения.
        public TaskManagerContext()
            : base("name=TaskManagerContext")
        {
        }
       public DbSet<Task> Tasks { get; set; }
      public  DbSet<Reiteration> Reiterations { get; set; }
       public DbSet<TypeOperation> TypeOperations { get; set; }

        // Добавьте DbSet для каждого типа сущности, который требуется включить в модель. Дополнительные сведения 
        // о настройке и использовании модели Code First см. в статье http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}