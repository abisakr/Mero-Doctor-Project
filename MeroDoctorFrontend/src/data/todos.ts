import type { Todo } from "../type/todo";

export const dummyData: Todo[] = [
    {
        id: 1,  
        title: "Buy groceries",
        description: "Milk, Bread, Eggs, Butter",
        completed: true,   
    },
    {
        id: 2,  
        title: "Walk the dog",
        description: "Take Fido for a walk in the park",
        completed: false,
    },
    {
        id: 3,
        title: "Finish project",
        description: "Complete the React project by Friday",
        completed: false,
    },
];
