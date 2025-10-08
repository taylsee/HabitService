# Habit Service 🎯

Микросервис для управления привычками пользователей.

## 🚀 Быстрый запуск в docker

### Development режим (для разработки)
```bash
docker-compose up -d
```
### Production режим (без swagger)
```bash
docker-compose -f docker-compose.yml -f docker-compose.vs.release.yml up -d
```

## Ссылки
### Через докер: http://localhost:8080/swagger

### В среде:
### https://localhost:7081/swagger  
### http://localhost:5248/swagger