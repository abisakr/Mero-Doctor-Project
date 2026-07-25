import { useState } from "react";
function GreetingForm(){
    const [name, setName] = useState('')
    const [isStudent, setIsStudent] = useState(false)

    return(
    <div>
          <input type="text" placeholder="Enter your Name" value={name} onChange={(e)=>setName(e.target.value)}/>
            <input type="checkbox" checked={isStudent} onChange={(e)=>setIsStudent(e.target.checked)}/>
            Click if You are a student
          
             <p>You typed: {name}</p>
             <p>{isStudent ? "You are a student." : "You are not a student."}</p>
        </div>
    )
}
export default GreetingForm;