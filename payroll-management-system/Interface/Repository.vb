Namespace Interfaces
    Public Interface Repository
        Function FindById(databaseManager As Manager.Mysql, id As Object)
        Function Collect(databaseManager As Manager.Mysql)
        Function Save(databaseManager As Manager.Mysql)
    End Interface

End Namespace
