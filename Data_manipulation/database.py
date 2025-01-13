import pandas as pd
from sqlalchemy import create_engine

# Load the CSV file
df = pd.read_csv('Electric_Vehicle_Population_Data.csv')
df.rename(columns={
    'VIN (1-10)': 'vin',
    'County': 'county',
    'City': 'city',
    'State': 'state',
    'Postal Code': 'postal_code',
    'Model Year': 'model_year',
    'Make': 'make',
    'Model': 'model',
    'Electric Vehicle Type': 'electric_vehicle_type',
    'Clean Alternative Fuel Vehicle (CAFV) Eligibility': 'cafv_eligibility',
    'Electric Range': 'electric_range',
    'Base MSRP': 'base_msrp',
    'Legislative District': 'legislative_district',
    'DOL Vehicle ID': 'dol_vehicle_id',
    'Vehicle Location': 'vehicle_location',
    'Electric Utility': 'electric_utility',
    '2020 Census Tract': 'census_tract'
}, inplace=True)

# Verify the renamed columns
print("Renamed Columns:", df.columns)

# Connect to postgreSQL
engine = create_engine('postgresql://postgres:Ku4eto114@localhost:5432/ev_database')
# LOAD THE DATA
df.to_sql('population', engine, if_exists='append', index=False)

print("Data loaded successfully!")