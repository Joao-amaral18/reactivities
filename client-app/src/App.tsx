import React, {useEffect, useState} from 'react';
import axios from 'axios';
import { Users } from '@phosphor-icons/react';

function App() {

  const [activities, setActivities] = useState([]);
  
  useEffect(() => {
    axios.get("http://localhost:5000/api/activities")
    .then(res => {
      setActivities(res.data)
      console.log(res)
    })
  },[]) 
  
  return (
    <div className="App mx-8 my-2">
      <header className="App-header">
          <div className="Header flex flex-row">
            <Users size={32} className="mr-2" />
            <h2 className='text-xl font-semibold'>Reactivities</h2>
            </div>
        <ul 
        className='leading-8'
        >
          {activities.map((activity:any) =>(
            <li key = {activity.id}>
              {activity.title}
            </li>
          ))}
        </ul>
      </header>
    </div>
  );
}

export default App;
