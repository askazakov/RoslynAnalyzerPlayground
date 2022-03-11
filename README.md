# RoslynAnalyzerPlayground

Пример приложения с Roslyn-анализатором.

Makefile содержит несколько целей:

`pack` – собирает nuget-пакет, который подключён к SampleProject

`clean` – удаляет nuget-пакет из кэша, так как после первого билда SampleProject пакет закэшируется и больше локальные правки не будут применяться к SampleProject (без изменений версии)