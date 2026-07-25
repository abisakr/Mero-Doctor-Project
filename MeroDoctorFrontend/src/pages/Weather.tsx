import { useState, useEffect } from 'react'

type WeatherData = {
  current_weather: {
    temperature: number
    windspeed: number
  }
}

function Weather() {
  const [data, setData] = useState<WeatherData | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    async function fetchWeather() {
      try {
        const response = await fetch(
          'https://api.open-meteo.com/v1/forecast?latitude=27.7&longitude=85.3&current_weather=true'
        )
        if (!response.ok) throw new Error('Failed to fetch')
        const json: WeatherData = await response.json()
        setData(json)
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Unknown error')
      } finally {
        setLoading(false)
      }
    }

    fetchWeather()
  }, []) // run once on mount

  if (loading) return <p>Loading weather...</p>
  if (error) return <p>Error: {error}</p>

  return (
    <div>
      <h2>Kathmandu Weather</h2>
      <p>Temperature: {data?.current_weather.temperature}°C</p>
      <p>Wind speed: {data?.current_weather.windspeed} km/h</p>
    </div>
  )
}

export default Weather