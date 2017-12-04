from suds.client import Client
sensorData = {}
response = 'OK'

class Broadcast

	def __init__(self, delayToSend):
		askForAcces()
		while(response=='OK'):
			pass
		else:
			print('Error')
		
	def postToSOAP(sensorData):
		data = client.factory.create('datapack') ##Creating the composite type DataPack
		data.IR1 = sensorData["IR1"]
		data.IR2 = sensorData["IR2"]
		data.Temperature = sensorData["Temp"]
		data.Humidity = sensorData["Humidity"]
		data.Name = sensorData["Name"]
		
		result = client.service.SaveData(data)	##TODO: See if there's a method to construct a method(object)
	
	def askForAcces()##TODO: Will take a token or smth
		client = Client('http://localhost:6876/Service.svc?wsdl') ##TODO: Implement wsAuth!