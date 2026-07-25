import { useState,useEffect  } from "react";
function Clock(){
const [time, setTime] = useState(new Date());
 
useEffect(() => {
    const timer = setInterval(() => {
        setTime(new Date());
    }, 1000);
    return () => clearInterval(timer);
}, []);
return(
    <div>
        <p className="text-lg font-semibold">Current Time: {time.toLocaleTimeString()}</p>
    </div>
)

}
export default Clock;