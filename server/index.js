const fs = require("fs/promises");
const path = require("path");
const express = require("express");
const cors = require("cors");
const app = express();
const PORT = 4000;
const DB_PATH = path.join(__dirname, "..", "db.json");
app.use(express.json());
app.use(cors());


async function readTodos() {
  const data = await fs.readFile(DB_PATH, "utf-8");
  const parsedData = JSON.parse(data);
  return parsedData.todos;
}

async function writeTodos(todos) {
  const data = {
    todos: todos,
  };

  await fs.writeFile(DB_PATH, JSON.stringify(data, null, 2));
}


app.get("/health", (req, res) => {
  res.json({ ok: true });
});

app.get("/todos", async (req, res) => {
  try {
    const todos = await readTodos();
    res.json(todos);
  } catch (error) {
    res.status(500).json({ message: "Failed to read todos" });
  }
});


app.post("/todos", async (req, res) => {
  try {
    const { title, description } = req.body;

    const todos = await readTodos();

    const newTodo = {
      id: Date.now().toString(),
      title,
      description,
      completed: false,
    };

    todos.push(newTodo);

    await writeTodos(todos);

    res.status(201).json(newTodo);
  } catch (error) {
    res.status(500).json({ message: "Failed to create todo" });
  }
});


app.delete("/todos/:id", async (req, res) => {
  try {
    const { id } = req.params;

    const todos = await readTodos();

    const updatedTodos = todos.filter((todo) => todo.id !== id);

    await writeTodos(updatedTodos);

    res.status(204).send();
  } catch (error) {
    res.status(500).json({ message: "Failed to delete todo" });
  }
});


app.patch("/todos/:id", async (req, res) => {
  try {
    const { id } = req.params;
    const { completed } = req.body;

    const todos = await readTodos();

    const updatedTodos = todos.map((todo) => {
      if (todo.id === id) {
        return {
          ...todo,
          completed: completed,
        };
      }

      return todo;
    });

    const updatedTodo = updatedTodos.find((todo) => todo.id === id);

    await writeTodos(updatedTodos);

    res.json(updatedTodo);
  } catch (error) {
    res.status(500).json({ message: "Failed to update todo" });
  }
});


app.listen(PORT, () => {
  console.log(`Server is running on http://localhost:${PORT}`);
});
