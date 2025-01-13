import pandas as pd
from sqlalchemy import create_engine
import matplotlib.pyplot as plt

# Connect to PostgreSQL
engine = create_engine('postgresql://postgres:Ku4eto114@localhost:5432/ev_database')

# First 30 rows query
query = """SELECT * FROM population LIMIT 30;"""

df_results = pd.read_sql_query(query, engine)

print("First 30 rows: ")
print(df_results)

# Query to count total records
query = """
SELECT COUNT(*) AS total_recrords FROM population;
"""
df_result = pd.read_sql_query(query, engine)

print(df_result)

# Query to count electric vehicles by make
query = """
SELECT make, COUNT(*) AS num_vehicles
FROM population
GROUP BY make
ORDER BY num_vehicles DESC;
"""
df_result = pd.read_sql_query(query, engine)
print(df_result)

# Plotting the result
plt.figure(figsize=(10, 6))
plt.bar(df_result['make'], df_result['num_vehicles'])
plt.title('Top 10 Electric Vehicle Makes by Count')
plt.xlabel('Make')
plt.ylabel('Number of Vehicles')
plt.xticks(rotation=45)
plt.show()


# Query to count electric vehicles by state
query = """
SELECT state,
COUNT(*) AS state_count FROM population
GROUP BY state
ORDER BY state_count DESC;
"""
df_result = pd.read_sql_query(query, engine)
print(df_result)

# Plotting the result
plt.figure(figsize=(10, 6))
plt.bar(df_result['state'], df_result['state_count'])
plt.title('Top 10 Electric Vehicle States by Count')
plt.xlabel('State')
plt.ylabel('Number of Vehicles')
plt.xticks(rotation=45)
plt.show()
